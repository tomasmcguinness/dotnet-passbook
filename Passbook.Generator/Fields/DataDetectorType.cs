using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Passbook.Generator.Fields
{
	[Flags]
	public enum DataDetectorTypes
	{
		PKDataDetectorAll = 0,
		PKDataDetectorNone = 1,
		PKDataDetectorTypePhoneNumber = 2,
		PKDataDetectorTypeLink = 4,
		PKDataDetectorTypeAddress = 8,
		PKDataDetectorTypeCalendarEvent = 16
	}
}
