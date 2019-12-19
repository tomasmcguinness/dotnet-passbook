namespace Passbook.Generator.Fields
{
    public enum FieldDateTimeStyle
    {
        Unspecified,
        /// <summary>
        /// Specifies no style.
        /// </summary>
        PKDateStyleNone,
        /// <summary>
        /// Specifies a short style, typically numeric only, such as "11/23/37" or "3:30 PM".
        /// </summary>
        PKDateStyleShort,
        /// <summary>
        /// Specifies a medium style, typically with abbreviated text, such as "Nov 23, 1937" or "3:30:32 PM".
        /// </summary>
        PKDateStyleMedium,
        /// <summary>
        /// Specifies a long style, typically with full text, such as "November 23, 1937" or "3:30:32 PM PST".
        /// </summary>
        PKDateStyleLong,
        /// <summary>
        /// Specifies a full style with complete details, such as "Tuesday, April 12, 1952 AD" or "3:30:42 PM Pacific Standard Time".
        /// </summary>
        PKDateStyleFull
    }
}
