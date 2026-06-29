using AutoMapper;
using Moq;
using Pixora.BL.DTOs;
using Pixora.BL.Models;
using Pixora.BL.Services.Hashtags;
using Pixora.BL.Services.ImageProcessing;
using Pixora.BL.Services.Logs;
using Pixora.BL.Services.Photos;
using Pixora.BL.Services.Plans;
using Pixora.BL.Services.Storage;
using Pixora.DAL.Models;
using Pixora.DAL.Repositories.Interfaces;

namespace Pixora.UnitTests;

public class PhotoServiceTests
{
    private const string UserId = "user-1";
    private const string OwnerId = "owner-id";
    private const string OtherUserId = "other-user-id";
    private const string AdminId = "admin-id";

    private const int PhotoId = 10000;
    private const int DeletePhotoId = 5;

    private readonly Mock<IPhotoRepository> _photoRepo = new();
    private readonly Mock<IPhotoHashtagRepository> _photoHashtagRepo = new();
    private readonly Mock<IApplicationUserRepository> _userRepo = new();
    private readonly Mock<IHashtagService> _hashtagService = new();
    private readonly Mock<IImageStorageService> _storageService = new();
    private readonly Mock<IImageProcessor> _imageProcessor = new();
    private readonly Mock<IHttpClientFactory> _httpClientFactory = new();
    private readonly Mock<IMapper> _mapper = new();

    private PhotoService CreateService()
    {
        var resolver = new PlanPolicyResolver(new List<IPlanPolicy>
        {
            new FreePlanPolicy(),
            new ProPlanPolicy(),
            new GoldPlanPolicy()
        });

        return new PhotoService(_photoRepo.Object, _photoHashtagRepo.Object, _userRepo.Object, _hashtagService.Object,
            _storageService.Object, _imageProcessor.Object, resolver, _httpClientFactory.Object, _mapper.Object
        );
    }

    [Fact]
    public async Task UploadPhotoAsync_Throws_WhenUserNotFound()
    {
        // Arrange
        var service = CreateService();

        _userRepo.Setup(r => r.GetById(UserId)).Returns((ApplicationUser?)null);

        var dto = new UploadPhotoDto
        {
            UserId = UserId,
            FileSizeBytes = long.MaxValue,
            ImageStream = new MemoryStream(),
            OriginalFileName = "photo.jpg",
            ProcessingOptions = new ProcessingOptions()
        };

        // Act
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.UploadPhotoAsync(dto));

        // Assert
        Assert.Equal("User not found.", ex.Message);

        _photoRepo.Verify(r => r.Save(), Times.Never);
        _storageService.Verify(s => s.UploadImageAsync(
            It.IsAny<Stream>(), 
            It.IsAny<string>(),
            It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task UploadPhotoAsync_Throws_WhenPackageLimitExceeded()
    {
        // Arrange
        var service = CreateService();

        _userRepo.Setup(r => r.GetById(UserId))
            .Returns(new ApplicationUser
            {
                Id = UserId,
                Email = "user@test.com",
                PlanType = PlanType.Free,
                StorageUsedBytes = 0
            });

        _photoRepo.Setup(r => r.GetByAuthorId(UserId)).Returns([]);

        var dto = new UploadPhotoDto
        {
            UserId = UserId,
            FileSizeBytes = long.MaxValue,
            ImageStream = new MemoryStream(),
            OriginalFileName = "huge.jpg",
            ProcessingOptions = new ProcessingOptions()
        };

        // Act
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.UploadPhotoAsync(dto));

        // Assert
        Assert.Equal("Package limit exceeded.", ex.Message);

        _photoRepo.Verify(r => r.Save(), Times.Never);
        _storageService.Verify(s => s.UploadImageAsync(
            It.IsAny<Stream>(), 
            It.IsAny<string>(), 
            It.IsAny<string>()), 
            Times.Never);
    }

    [Fact]
    public void EditMetadata_Throws_WhenUserEditsAnotherUsersPhoto()
    {
        // Arrange
        var service = CreateService();

        _photoRepo.Setup(r => r.GetById(PhotoId))
            .Returns(new Photo
            {
                Id = PhotoId,
                AuthorId = OwnerId,
                Description = "Old description"
            });

        var dto = new EditPhotoMetadataDto
        {
            PhotoId = PhotoId,
            UserId = OtherUserId,
            Description = "Edited description",
            Hashtags = ["nature"]
        };

        // Act
        var ex = Assert.Throws<UnauthorizedAccessException>(() => service.EditMetadata(dto, isAdmin: false));

        // Assert
        Assert.Equal("You can edit only your own photos.", ex.Message);

        _photoRepo.Verify(r => r.Update(It.IsAny<Photo>()), Times.Never);
        _photoRepo.Verify(r => r.Save(), Times.Never);
    }

    [Fact]
    public void EditMetadata_Allows_AdminToEditAnyPhoto()
    {
        // Arrange
        var service = CreateService();

        var photo = new Photo
        {
            Id = PhotoId,
            AuthorId = OwnerId,
            Description = "Old description"
        };

        _photoRepo.Setup(r => r.GetById(PhotoId)).Returns(photo);

        _hashtagService.Setup(h => h.GetOrCreateMany(It.IsAny<IEnumerable<string>>()))
            .Returns([
                new Hashtag
                {
                    Id = 1,
                    Name = "nature"
                }
            ]);

        var dto = new EditPhotoMetadataDto
        {
            PhotoId = PhotoId,
            UserId = AdminId,
            Description = "Changed by admin",
            Hashtags = ["nature"]
        };

        // Act
        service.EditMetadata(dto, isAdmin: true);

        // Assert
        Assert.Equal("Changed by admin", photo.Description);

        _hashtagService.Verify(h => h.GetOrCreateMany(It.IsAny<IEnumerable<string>>()), Times.Once);

        _photoHashtagRepo.Verify(r => r.DeleteByPhotoId(PhotoId), Times.Once);
        _photoRepo.Verify(r => r.Update(photo), Times.Once);
        _photoRepo.Verify(r => r.Save(), Times.Once);
        _photoHashtagRepo.Verify(r => r.Save(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Throws_WhenUserDeletesAnotherUsersPhoto()
    {
        // Arrange
        var service = CreateService();

        _photoRepo.Setup(r => r.GetById(DeletePhotoId))
            .Returns(new Photo
            {
                Id = DeletePhotoId,
                AuthorId = OwnerId,
                ImagePath = "https://example.com/photo.jpg",
                FileSizeBytes = 1000
            });

        // Act
        var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.DeleteAsync(DeletePhotoId, OtherUserId, isAdmin: false));

        // Assert
        Assert.Equal("You can delete only your own photos.", ex.Message);

        _photoRepo.Verify(r => r.Delete(It.IsAny<int>()), Times.Never);
        _photoRepo.Verify(r => r.Save(), Times.Never);
        _storageService.Verify(s => s.DeleteImageAsync(It.IsAny<string>()), Times.Never);
    }
}