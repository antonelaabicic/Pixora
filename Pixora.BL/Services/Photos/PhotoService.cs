using AutoMapper;
using Pixora.BL.DTOs;
using Pixora.BL.Services.Hashtags;
using Pixora.BL.Services.ImageProcessing;
using Pixora.BL.Services.Logs;
using Pixora.BL.Services.Plans;
using Pixora.BL.Services.Storage;
using Pixora.DAL.Models;
using Pixora.DAL.Repositories.Interfaces;
using System.Net.Http;

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
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly PlanPolicyResolver _planPolicyResolver;

        private readonly IMapper _mapper;

        public PhotoService(IPhotoRepository photoRepository, IPhotoHashtagRepository photoHashtagRepository,
            IApplicationUserRepository userRepository, IHashtagService hashtagService,
            IImageStorageService storageService, IImageProcessor imageProcessor,
            PlanPolicyResolver planPolicyResolver, IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _photoRepository = photoRepository;
            _photoHashtagRepository = photoHashtagRepository;
            _userRepository = userRepository;
            _hashtagService = hashtagService;
            _storageService = storageService;
            _imageProcessor = imageProcessor;
            _planPolicyResolver = planPolicyResolver;
            _httpClientFactory = httpClientFactory;
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

            if (string.IsNullOrWhiteSpace(extension) || extension == "string")
            {
                extension = Path.GetExtension(dto.OriginalFileName)
                    .TrimStart('.')
                    .ToLower();
            }

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
                dto.UploadedTo, dto.AuthorEmail);
        }

        public void EditMetadata(EditPhotoMetadataDto dto, bool isAdmin = false)
        {
            var photo = _photoRepository.GetById(dto.PhotoId) ?? throw new InvalidOperationException("Photo not found.");

            if (!isAdmin && photo.AuthorId != dto.UserId)
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
        }

        public async Task<DownloadPhotoDto> DownloadAsync(int photoId)
        {
            var photo = _photoRepository.GetById(photoId) ?? throw new InvalidOperationException("Photo not found.");

            var client = _httpClientFactory.CreateClient("Downloads");
            var stream = await client.GetStreamAsync(photo.ImagePath);

            var extension = Path.GetExtension(photo.ImagePath);

            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = ".jpg";
            }

            return new DownloadPhotoDto
            {
                Stream = stream,
                FileName = $"photo-{photo.Id}{extension}",
                ContentType = GetContentType(extension)
            };
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