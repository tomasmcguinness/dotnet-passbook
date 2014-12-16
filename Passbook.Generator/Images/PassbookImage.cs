using System;

namespace Passbook.Generator
{
    public enum PassbookImage
    {
        [StringValue("background.png")]
        Background,
        [StringValue("background@2x.png")]
        BackgroundRetina,
        [StringValue("icon.png")]
        Icon,
        [StringValue("icon@2x.png")]
        IconRetina,
        [StringValue("logo.png")]
        Logo,
        [StringValue("logo@2x.png")]
        LogoRetina,
        [StringValue("strip.png")]
        Strip,
        [StringValue("strip@2x.png")]
        StripRetina,
        [StringValue("thumbnail@2x.png")]
        Thumbnail,
        [StringValue("thumbnail@2x.png")]
        ThumbnailRetina,
        [StringValue("footer.png")]
        Footer,
        [StringValue("footer@2x.png")]
        FooterRetina
    }
}
