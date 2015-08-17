using System;
using System.Collections.Generic;

namespace Passbook.Web.Serialization
{
	public class UpdatedPasses
	{
		public string lastUpdated
		{
			get;
			set;
		}

		public List<string> serialNumbers
		{
			get;
			set;
		}
	}
}

