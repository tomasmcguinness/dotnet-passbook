using System.IO.Enumeration;
using Xunit;

namespace Passbook.Generator.Tests;

public class PassbookImageTests
{
    [Theory]
    [
        InlineData("icon.png", PassbookImage.Icon),
        InlineData("icon@2x.png", PassbookImage.Icon2X),
        InlineData("icon@3x.png", PassbookImage.Icon3X),
        InlineData("logo.png", PassbookImage.Logo),
        InlineData("logo@2x.png", PassbookImage.Logo2X),
        InlineData("logo@3x.png", PassbookImage.Logo3X),
        InlineData("background.png", PassbookImage.Background),
        InlineData("background@2x.png", PassbookImage.Background2X),
        InlineData("background@3x.png", PassbookImage.Background3X),
        InlineData("strip.png", PassbookImage.Strip),
        InlineData("strip@2x.png", PassbookImage.Strip2X),
        InlineData("strip@3x.png", PassbookImage.Strip3X),
        InlineData("thumbnail.png", PassbookImage.Thumbnail),
        InlineData("thumbnail@2x.png", PassbookImage.Thumbnail2X),
        InlineData("thumbnail@3x.png", PassbookImage.Thumbnail3X),
        InlineData("footer.png", PassbookImage.Footer),
        InlineData("footer@2x.png", PassbookImage.Footer2X),
        InlineData("footer@3x.png", PassbookImage.Footer3X)
    ]
    public void ToFilename_ReturnsCorrectFilenames(string expectedFilename, PassbookImage passbookImage)
    {
        string actualFilename = passbookImage.ToFilename();
        Assert.Equal(expectedFilename, actualFilename);
    }
}
