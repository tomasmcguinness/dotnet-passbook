using System;

namespace Passbook.Generator.Fields
{
    /// <summary>
    /// Barcode format
    /// </summary>
    public enum BarcodeType
    {
        None = 0,
        PKBarcodeFormatQR = 1,
        PKBarcodeFormatPDF417,
        PKBarcodeFormatAztec,
        PKBarcodeFormatText,
    } 
}
