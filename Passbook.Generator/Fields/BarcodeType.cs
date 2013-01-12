using System;

namespace Passbook.Generator.Fields
{
    /// <summary>
    /// Barcode format
    /// </summary>
    public enum BarcodeType
    {
        PKBarcodeFormatQR,
        PKBarcodeFormatPDF417,
        PKBarcodeFormatAztec,
        PKBarcodeFormatText
    }
}
