namespace Passbook.Generator.Fields
{
    public enum FieldNumberStyle
    {
        Unspecified,
        /// <summary>
        /// Specifies a decimal style format.
        /// </summary>
        PKNumberStyleDecimal,
        /// <summary>
        /// Specifies a currency style format.
        /// </summary>
        PKNumberStylePercent,
        /// <summary>
        /// Specifies a percent style format.
        /// </summary>
        PKNumberStyleScientific,
        /// <summary>
        /// Specifies a spell-out format; for example, "23" becomes "twenty-three".
        /// </summary>
        PKNumberStyleSpellOut
    }
}
