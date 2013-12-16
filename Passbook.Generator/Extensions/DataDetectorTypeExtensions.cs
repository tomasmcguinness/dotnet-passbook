using Passbook.Generator.Fields;

namespace Passbook.Generator
{
    public static class DataDetectorTypeExtensions
    {
        // Dotfucated friendly enum name conversion
        // http://stackoverflow.com/questions/483794/convert-enum-to-string
        public static string ToSafeString(this DataDetectorType dataDetectorType)
        {
            switch (dataDetectorType)
            {
                case DataDetectorType.PKDataDetectorTypeAddress:
                    return "PKDataDetectorTypeAddress";
                case DataDetectorType.PKDataDetectorTypeCalendarEvent:
                    return "PKDataDetectorTypeCalendarEvent";
                case DataDetectorType.PKDataDetectorTypeLink:
                    return "PKDataDetectorTypeLink";
                case DataDetectorType.PKDataDetectorTypePhoneNumber:
                    return "PKDataDetectorTypePhoneNumber";
                default:
                    return "";
            }
        }
    }
}
