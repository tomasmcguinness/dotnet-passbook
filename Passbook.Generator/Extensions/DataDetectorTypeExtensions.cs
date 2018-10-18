using Passbook.Generator.Fields;

namespace Passbook.Generator
{
    public static class DataDetectorTypeExtensions
    {
        // Dotfucated friendly enum name conversion
        // http://stackoverflow.com/questions/483794/convert-enum-to-string
        public static string ToSafeString(this DataDetectorTypes dataDetectorType)
        {
            switch (dataDetectorType)
            {
                case DataDetectorTypes.PKDataDetectorTypeAddress:
                    return "PKDataDetectorTypeAddress";
                case DataDetectorTypes.PKDataDetectorTypeCalendarEvent:
                    return "PKDataDetectorTypeCalendarEvent";
                case DataDetectorTypes.PKDataDetectorTypeLink:
                    return "PKDataDetectorTypeLink";
                case DataDetectorTypes.PKDataDetectorTypePhoneNumber:
                    return "PKDataDetectorTypePhoneNumber";
                default:
                    return "";
            }
        }
    }
}
