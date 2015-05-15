using System;

namespace Passbook.Generator
{
	public static class PassbookImageExtensions
	{
		public static string ToFilename(this PassbookImage passbookImage)
		{
			switch (passbookImage)
			{
			case PassbookImage.Icon:
				return "icon.png";
			case PassbookImage.IconRetina:
				return "icon@2x.png";
			case PassbookImage.Logo:
				return "logo.png";
			case PassbookImage.LogoRetina:
				return "logo@2x.png";
			case PassbookImage.Background:
				return "background.png";
			case PassbookImage.BackgroundRetina:
				return "background@2x.png";
			case PassbookImage.Strip:
				return "strip.png";
			case PassbookImage.StripRetina:
				return "strip@2x.png";
			case PassbookImage.Thumbnail:
				return "thumbnail.png";
			case PassbookImage.ThumbnailRetina:
				return "thumbnail@2x.png";
			case PassbookImage.Footer:
				return "footer.png";
			case PassbookImage.FooterRetina:
				return "footer@2x.png";
			default:
				throw new NotImplementedException("Unknown PassbookImage type.");
			}
		}
	}

    public enum PassbookImage
    {
        Background,
        BackgroundRetina,
        Icon,
        IconRetina,
        Logo,
        LogoRetina,
        Strip,
        StripRetina,
        Thumbnail,
        ThumbnailRetina,
        Footer,
        FooterRetina
    }
}
