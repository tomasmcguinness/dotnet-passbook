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
			Value = (T)Convert.ChangeType(content, typeof(T));
		}
	}
}

