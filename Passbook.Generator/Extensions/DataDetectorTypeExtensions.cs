using Passbook.Generator.Fields;

namespace Passbook.Generator;

public static class DataDetectorTypeExtensions
{
    // Dotfuscated friendly enum name conversion
    // http://stackoverflow.com/questions/483794/convert-enum-to-string
    public static string ToSafeString(this DataDetectorTypes dataDetectorType)
    {
        return dataDetectorType switch
        {
            DataDetectorTypes.PKDataDetectorTypeAddress => "PKDataDetectorTypeAddress",
            DataDetectorTypes.PKDataDetectorTypeCalendarEvent => "PKDataDetectorTypeCalendarEvent",
            DataDetectorTypes.PKDataDetectorTypeLink => "PKDataDetectorTypeLink",
            DataDetectorTypes.PKDataDetectorTypePhoneNumber => "PKDataDetectorTypePhoneNumber",
            _ => "",
        };
    }
}
