using System;

namespace Passbook.Generator.Fields
{
    [Flags]
    public enum DataDetectorTypes
    {
        /// <summary>
        /// Automatically detect any of the supported data types
        /// </summary>
        PKDataDetectorAll = 0,
        /// <summary>
        /// Do not detect any data types
        /// </summary>
        PKDataDetectorNone = 1,
        /// <summary>
        /// Automatically detect phone numbers
        /// </summary>
        PKDataDetectorTypePhoneNumber = 2,
        /// <summary>
        /// Automatically detect links
        /// </summary>
        PKDataDetectorTypeLink = 4,
        /// <summary>
        /// Automatically detect addresses
        /// </summary>
        PKDataDetectorTypeAddress = 8,
        /// <summary>
        /// Automatically detect calendar events
        /// </summary>
        PKDataDetectorTypeCalendarEvent = 16
    }
}
