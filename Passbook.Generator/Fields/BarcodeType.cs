namespace Passbook.Generator.Fields
{
    /// <summary>
    /// Barcode format
    /// </summary>
    public enum BarcodeType
    {
        /// <summary>
        /// No barcode format
        /// </summary>
        None = 0,
        /// <summary>
        /// QRCode
        /// </summary>
        PKBarcodeFormatQR = 1,
        /// <summary>
        /// PDF-417
        /// </summary>
        PKBarcodeFormatPDF417,
        /// <summary>
        /// Aztec
        /// </summary>
        PKBarcodeFormatAztec,
        /// <summary>
        /// Text
        /// </summary>
        PKBarcodeFormatText,
        /// <summary>
		/// Code-128
		/// </summary>
        PKBarcodeFormatCode128
    }
}
