namespace Passbook.Generator.Fields
{
    /// <summary>
    /// Barcode format
    /// </summary>
    public enum BarcodeType
    {
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
        /// Code128
        /// </summary>
        PKBarcodeFormatCode128,
    }
}
