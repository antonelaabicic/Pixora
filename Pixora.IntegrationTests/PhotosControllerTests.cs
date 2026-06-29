using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Pixora.BL.DTOs;

namespace Pixora.IntegrationTests;

public class PhotosControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PhotosControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetById_ReturnsExpectedPhoto()
    {
        // Arrange
        int photoId = 1;

        // Act
        var response = await _client.GetAsync($"/api/photos/{photoId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var photo = await response.Content.ReadFromJsonAsync<PhotoDto>();

        photo.Should().NotBeNull();
        photo!.Id.Should().Be(photoId);
        photo.Description.Should().Be("Pretty grey parrot.");
    }

    [Fact]
    public async Task Search_ReturnsPhotosMatchingHashtag()
    {
        // Arrange
        var dto = new PhotoSearchDto
        {
            Hashtag = "nature"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/photos/search", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var photos = await response.Content.ReadFromJsonAsync<List<PhotoDto>>();

        photos.Should().NotBeNull();
        photos.Should().HaveCount(2);
        photos.Should().Contain(p => p.Id == 5);
        photos.Should().Contain(p => p.Id == 9);
    }

    [Fact]
    public async Task GetLatest_ReturnsRequestedNumberOfPhotos()
    {
        // Arrange
        int count = 2;

        // Act
        var response = await _client.GetAsync($"/api/photos/latest?count={count}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var photos = await response.Content.ReadFromJsonAsync<List<PhotoDto>>();

        photos.Should().NotBeNull();
        photos.Should().HaveCount(count);
    }

    [Fact]
    public async Task GetAll_ReturnsAllPhotos()
    {
        // Act
        var response = await _client.GetAsync("/api/photos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var photos = await response.Content.ReadFromJsonAsync<List<PhotoDto>>();

        photos.Should().NotBeNull();
        photos.Should().NotBeEmpty();
    }
}