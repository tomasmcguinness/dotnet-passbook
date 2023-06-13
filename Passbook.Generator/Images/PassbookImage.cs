using System;

namespace Passbook.Generator
{
    public static class PassbookImageExtensions
    {
        public static string ToFilename(this PassbookImage passbookImage)
        {
            String filename;
            switch (passbookImage.Type)
            {
                case PassbookImageType.Icon:
                    filename = "icon.png";
                    break;
                case PassbookImageType.Icon2X:
                    filename = "icon@2x.png";
                    break;
                case PassbookImageType.Icon3X:
                    filename = "icon@3x.png";
                    break;
                case PassbookImageType.Logo:
                    filename = "logo.png";
                    break;
                case PassbookImageType.Logo2X:
                    filename = "logo@2x.png";
                    break;
                case PassbookImageType.Logo3X:
                    filename = "logo@3x.png";
                    break;
                case PassbookImageType.Background:
                    filename = "background.png";
                    break;
                case PassbookImageType.Background2X:
                    filename = "background@2x.png";
                    break;
                case PassbookImageType.Background3X:
                    filename = "background@3x.png";
                    break;
                case PassbookImageType.Strip:
                    filename = "strip.png";
                    break;
                case PassbookImageType.Strip2X:
                    filename = "strip@2x.png";
                    break;
                case PassbookImageType.Strip3X:
                    filename = "strip@3x.png";
                    break;
                case PassbookImageType.Thumbnail:
                    filename = "thumbnail.png";
                    break;
                case PassbookImageType.Thumbnail2X:
                    filename = "thumbnail@2x.png";
                    break;
                case PassbookImageType.Thumbnail3X:
                    filename = "thumbnail@3x.png";
                    break;
                case PassbookImageType.Footer:
                    filename = "footer.png";
                    break;
                case PassbookImageType.Footer2X:
                    filename = "footer@2x.png";
                    break;
                case PassbookImageType.Footer3X:
                    filename = "footer@3x.png";
                    break;
                default:
                    throw new NotImplementedException("Unknown PassbookImage type.");
            }
            if (!String.IsNullOrEmpty(passbookImage.Culture))
            {
                filename = passbookImage.Culture + "/" + filename;
            }
            return filename;
        }
    }

    public struct PassbookImage
    {
        public PassbookImageType Type { get; set; }
        public String LanguageCode { get; set; }

        public static readonly PassbookImage Background = new PassbookImage() { Type = PassbookImageType.Background };
        public static readonly PassbookImage Background2X = new PassbookImage() { Type = PassbookImageType.Background2X };
        public static readonly PassbookImage Background3X = new PassbookImage() { Type = PassbookImageType.Background3X };
        public static readonly PassbookImage Icon = new PassbookImage() { Type = PassbookImageType.Icon };
        public static readonly PassbookImage Icon2X = new PassbookImage() { Type = PassbookImageType.Icon2X };
        public static readonly PassbookImage Icon3X = new PassbookImage() { Type = PassbookImageType.Icon3X };
        public static readonly PassbookImage Logo = new PassbookImage() { Type = PassbookImageType.Logo };
        public static readonly PassbookImage Logo2X = new PassbookImage() { Type = PassbookImageType.Logo2X };
        public static readonly PassbookImage Logo3X = new PassbookImage() { Type = PassbookImageType.Logo3X };
        public static readonly PassbookImage Strip = new PassbookImage() { Type = PassbookImageType.Strip };
        public static readonly PassbookImage Strip2X = new PassbookImage() { Type = PassbookImageType.Strip2X };
        public static readonly PassbookImage Strip3X = new PassbookImage() { Type = PassbookImageType.Strip3X };
        public static readonly PassbookImage Thumbnail = new PassbookImage() { Type = PassbookImageType.Thumbnail };
        public static readonly PassbookImage Thumbnail2X = new PassbookImage() { Type = PassbookImageType.Thumbnail2X };
        public static readonly PassbookImage Thumbnail3X = new PassbookImage() { Type = PassbookImageType.Thumbnail3X };
        public static readonly PassbookImage Footer = new PassbookImage() { Type = PassbookImageType.Footer };
        public static readonly PassbookImage Footer2X = new PassbookImage() { Type = PassbookImageType.Footer2X };
        public static readonly PassbookImage Footer3X = new PassbookImage() { Type = PassbookImageType.Footer3X };
    }

    public enum PassbookImageType
    {
        /// <summary>
        /// Background image, 180x220 points
        /// </summary>
        Background,
        /// <summary>
        /// @2x Retina background image, 180x220 points
        /// </summary>
        Background2X,
        /// <summary>
        /// @3x Retina background image, 180x220 points
        /// </summary>
        Background3X,
        /// <summary>
        /// Icon as per https://developer.apple.com/library/ios/documentation/UserExperience/Conceptual/MobileHIG/Alerts.html#//apple_ref/doc/uid/TP40006556-CH14
        /// </summary>
        Icon,
        /// <summary>
        /// @2x Retina icon as per https://developer.apple.com/library/ios/documentation/UserExperience/Conceptual/MobileHIG/Alerts.html#//apple_ref/doc/uid/TP40006556-CH14
        /// </summary>
        Icon2X,
        /// <summary>
        /// Retina icon as per https://developer.apple.com/library/ios/documentation/UserExperience/Conceptual/MobileHIG/Alerts.html#//apple_ref/doc/uid/TP40006556-CH14
        /// </summary>
        Icon3X,
        /// <summary>
        /// Logo, 160x50 points
        /// </summary>
        Logo,
        /// <summary>
        /// @2x Retina logo, 160x50 points
        /// </summary>
        Logo2X,
        /// <summary>
        /// @3x Retina logo, 160x50 points
        /// </summary>
        Logo3X,
        /// <summary>
        /// Strip
        /// <list type="bullet">
        ///		<item>
        ///			<description>On iPhone 6 and 6 Plus The allotted space is 375x98 points for event tickets, 375x144 points for gift cards and coupons, and 375x123 in all other cases.</description>
        ///		</item>
        ///		<item>
        ///			<description>On prior hardware The allotted space is 320x84 points for event tickets, 320x110 points for other pass styles with a square barcode on devices with 3.5 inch screens, and 320x123 in all other cases.</description>
        ///		</item>
        ///		<item>
        ///			<description>On iOS 6 The strip image is only 312 points wide; the height varies as described above. A shine effect is automatically applied to the strip image; to suppress it use the suppressStripShine key.</description>
        ///		</item>
        /// </list>
        /// </summary>
        Strip,
        /// <summary>
        /// @2x Retina strip
        /// <list type="bullet">
        ///		<item>
        ///			<description>On iPhone 6 and 6 Plus The allotted space is 375x98 points for event tickets, 375x144 points for gift cards and coupons, and 375x123 in all other cases.</description>
        ///		</item>
        ///		<item>
        ///			<description>On prior hardware The allotted space is 320x84 points for event tickets, 320x110 points for other pass styles with a square barcode on devices with 3.5 inch screens, and 320x123 in all other cases.</description>
        ///		</item>
        ///		<item>
        ///			<description>On iOS 6 The strip image is only 312 points wide; the height varies as described above. A shine effect is automatically applied to the strip image; to suppress it use the suppressStripShine key.</description>
        ///		</item>
        /// </list>
        /// </summary>
        Strip2X,
        /// <summary>
        /// @3x Retina strip
        /// <list type="bullet">
        ///     <item>
        ///         <description>On iPhone 6 and 6 Plus The allotted space is 375x98 points for event tickets, 375x144 points for gift cards and coupons, and 375x123 in all other cases.</description>
        ///     </item>
        ///     <item>
        ///         <description>On prior hardware The allotted space is 320x84 points for event tickets, 320x110 points for other pass styles with a square barcode on devices with 3.5 inch screens, and 320x123 in all other cases.</description>
        ///     </item>
        ///     <item>
        ///         <description>On iOS 6 The strip image is only 312 points wide; the height varies as described above. A shine effect is automatically applied to the strip image; to suppress it use the suppressStripShine key.</description>
        ///     </item>
        /// </list>
        /// </summary>
        Strip3X,
        /// <summary>
        /// Thumbnail, 90x90 points
        /// </summary>
        Thumbnail,
        /// <summary>
        /// @2x Retina thumbnail, 90x90 points
        /// </summary>
        Thumbnail2X,
        /// <summary>
        /// @3x Retina thumbnail, 90x90 points
        /// </summary>
        Thumbnail3X,
        /// <summary>
        /// Footer, 286x15 points
        /// </summary>
        Footer,
        /// <summary>
        /// @2x Retina footer, 286x15 points
        /// </summary>
        Footer2X,
        /// <summary>
        /// @3x Retina footer, 286x15 points
        /// </summary>
        Footer3X
    }
}
