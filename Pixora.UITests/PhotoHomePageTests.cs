using Microsoft.Playwright;
using Xunit;

namespace Pixora.UITests;

public class PhotoHomePageTests : IAsyncLifetime
{
    private IPlaywright _playwright = null!;
    private IBrowser _browser = null!;
    private IPage _page = null!;

    private const string BaseUrl = "https://localhost:7136";

    public async Task InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();

        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });

        _page = await _browser.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        await _browser.CloseAsync();
        _playwright.Dispose();
    }

    [Fact]
    public async Task Search_ReturnsPhotosMatchingHashtag()
    {
        // Arrange
        await _page.GotoAsync(BaseUrl);

        // Act
        await _page.GetByTestId("search-hashtag").FillAsync("nature");
        await _page.GetByTestId("search-button").ClickAsync();

        // Assert
        await Assertions.Expect(_page.GetByText("#nature").First).ToBeVisibleAsync();
    }

    [Fact]
    public async Task ClickingPhoto_OpensAndClosesModal()
    {
        // Arrange
        await _page.GotoAsync(BaseUrl);
        await Assertions.Expect(_page.GetByTestId("photo-image").First).ToBeVisibleAsync();

        // Act
        await _page.GetByTestId("photo-image").First.ClickAsync();

        // Assert
        await Assertions.Expect(_page.GetByTestId("photo-modal")).ToBeVisibleAsync();

        // Act
        await _page.GetByTestId("close-photo-modal").ClickAsync();

        // Assert
        await Assertions.Expect(_page.GetByTestId("photo-modal")).ToBeHiddenAsync();
    }
}