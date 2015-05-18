using System;
using System.Configuration;
using System.Xml;

namespace Passbook.Generator.Configuration
{
	public class ConfigurationProperty<T> : ConfigurationElement
	{
		public T Value { get; private set; }

		public ConfigurationProperty()
		{
		}

		protected override void DeserializeElement(XmlReader reader, bool s) 
		{
			String content = reader.ReadElementContentAsString();

			if (!String.IsNullOrEmpty(content))
				Value = (T)Convert.ChangeType(content, typeof(T));
			else
				Value = default(T);
		}
	}
}

