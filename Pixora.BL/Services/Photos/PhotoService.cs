using AutoMapper;
using Pixora.BL.DTOs;
using Pixora.BL.Services.Hashtags;
using Pixora.BL.Services.ImageProcessing;
using Pixora.BL.Services.Logs;
using Pixora.BL.Services.Plans;
using Pixora.BL.Services.Storage;
using Pixora.DAL.Models;
using Pixora.DAL.Repositories.Interfaces;

namespace Pixora.BL.Services.Photos
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IPhotoHashtagRepository _photoHashtagRepository;
        private readonly IApplicationUserRepository _userRepository;
        private readonly IHashtagService _hashtagService;
        private readonly IImageStorageService _storageService;
        private readonly IImageProcessor _imageProcessor;
        private readonly PlanPolicyResolver _planPolicyResolver;
        private readonly IUserActionLogService _logService;
        private readonly IMapper _mapper;

        public PhotoService(IPhotoRepository photoRepository, IPhotoHashtagRepository photoHashtagRepository,
            IApplicationUserRepository userRepository, IHashtagService hashtagService,
            IImageStorageService storageService, IImageProcessor imageProcessor,
            PlanPolicyResolver planPolicyResolver, IUserActionLogService logService, IMapper mapper)
        {
            _photoRepository = photoRepository;
            _photoHashtagRepository = photoHashtagRepository;
            _userRepository = userRepository;
            _hashtagService = hashtagService;
            _storageService = storageService;
            _imageProcessor = imageProcessor;
            _planPolicyResolver = planPolicyResolver;
            _logService = logService;
            _mapper = mapper;
        }

        public async Task<int> UploadPhotoAsync(UploadPhotoDto dto)
        {
            var user = _userRepository.GetById(dto.UserId) ?? throw new InvalidOperationException("User not found.");

            var uploadsToday = _photoRepository
                .GetByAuthorId(dto.UserId)
                .Count(p => p.UploadedAt.Date == DateTime.UtcNow.Date);

            var policy = _planPolicyResolver.Resolve(user.PlanType);

            if (!policy.CanUpload(dto.FileSizeBytes, uploadsToday, user.StorageUsedBytes))
            {
                throw new InvalidOperationException("Package limit exceeded.");
            }

            var processedStream = _imageProcessor.Process(dto.ImageStream, dto.ProcessingOptions);

            var extension = dto.ProcessingOptions.OutputFormat;

            var imageUrl = await _storageService.UploadImageAsync(processedStream, extension, GetContentType(extension));

            var photo = _mapper.Map<Photo>(dto);
            photo.ImagePath = imageUrl;

            _photoRepository.Insert(photo);
            _photoRepository.Save();

            var hashtagEntities = _hashtagService.GetOrCreateMany(dto.Hashtags);

            foreach (var hashtag in hashtagEntities)
            {
                _photoHashtagRepository.Insert(new PhotoHashtag
                {
                    PhotoId = photo.Id,
                    HashtagId = hashtag.Id
                });
            }

            _photoHashtagRepository.Save();

            user.StorageUsedBytes += dto.FileSizeBytes;
            _userRepository.Update(user);
            _userRepository.Save();

            _logService.Log(dto.UserId, UserActionType.UploadedPhoto, $"Uploaded photo {photo.Id}.");

            return photo.Id;
        }

        public IEnumerable<Photo> GetAll()
        {
            return _photoRepository.GetAll();
        }

        public IEnumerable<Photo> GetLatest(int count)
        {
            return _photoRepository.GetLatest(count);
        }

        public Photo? GetById(int id)
        {
            return _photoRepository.GetById(id);
        }

        public IEnumerable<Photo> GetByAuthorId(string authorId)
        {
            return _photoRepository.GetByAuthorId(authorId);
        }

        public IEnumerable<Photo> Search(PhotoSearchDto dto)
        {
            return _photoRepository.Search(dto.Hashtag, dto.MinSizeBytes, dto.MaxSizeBytes, dto.UploadedFrom,
                dto.UploadedTo, dto.AuthorId);
        }

        public void EditMetadata(EditPhotoMetadataDto dto)
        {
            var photo = _photoRepository.GetById(dto.PhotoId) ?? throw new InvalidOperationException("Photo not found.");

            if (photo.AuthorId != dto.UserId)
            {
                throw new UnauthorizedAccessException("You can edit only your own photos.");
            }

            photo.Description = dto.Description;

            _photoHashtagRepository.DeleteByPhotoId(dto.PhotoId);

            var hashtagEntities = _hashtagService.GetOrCreateMany(dto.Hashtags);

            foreach (var hashtag in hashtagEntities)
            {
                _photoHashtagRepository.Insert(new PhotoHashtag
                {
                    PhotoId = photo.Id,
                    HashtagId = hashtag.Id
                });
            }

            _photoRepository.Update(photo);
            _photoRepository.Save();
            _photoHashtagRepository.Save();

            _logService.Log(dto.UserId, UserActionType.EditedPhoto, $"Edited photo {photo.Id}.");
        }

        public async Task DeleteAsync(int photoId, string userId, bool isAdmin = false)
        {
            var photo = _photoRepository.GetById(photoId) ?? throw new InvalidOperationException("Photo not found.");

            if (!isAdmin && photo.AuthorId != userId)
            {
                throw new UnauthorizedAccessException("You can delete only your own photos.");
            }

            await _storageService.DeleteImageAsync(photo.ImagePath);

            _photoRepository.Delete(photoId);
            _photoRepository.Save();

            var user = _userRepository.GetById(photo.AuthorId);

            if (user != null)
            {
                user.StorageUsedBytes = Math.Max(0, user.StorageUsedBytes - photo.FileSizeBytes);
                _userRepository.Update(user);
                _userRepository.Save();
            }

            _logService.Log(userId, isAdmin ? UserActionType.AdminDeletedPhoto : UserActionType.DeletedPhoto,
                $"Deleted photo {photoId}.");
        }

        public async Task<Stream> DownloadOriginalAsync(int photoId)
        {
            var photo = _photoRepository.GetById(photoId) ?? throw new InvalidOperationException("Photo not found.");

            using var httpClient = new HttpClient();
            var stream = await httpClient.GetStreamAsync(photo.ImagePath);

            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            _logService.Log(photo.AuthorId, UserActionType.DownloadedOriginalPhoto, $"Downloaded original photo {photo.Id}.");

            return memoryStream;
        }

        public async Task<Stream> DownloadProcessedAsync(DownloadProcessedPhotoDto dto)
        {
            var photo = _photoRepository.GetById(dto.PhotoId)
                ?? throw new InvalidOperationException("Photo not found.");

            using var httpClient = new HttpClient();
            var stream = await httpClient.GetStreamAsync(photo.ImagePath);

            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            var processedStream = _imageProcessor.Process(memoryStream, dto.ProcessingOptions);

            _logService.Log(
                photo.AuthorId,
                UserActionType.DownloadedProcessedPhoto,
                $"Downloaded processed photo {photo.Id}.");

            return processedStream;
        }

        private static string GetContentType(string extension)
        {
            return extension.ToLower() switch
            {
                "png" => "image/png",
                "bmp" => "image/bmp",
                "jpg" => "image/jpeg",
                "jpeg" => "image/jpeg",
                _ => "image/jpeg"
            };
        }
    }
}