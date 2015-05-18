using System;
using System.Collections.Generic;
using System.Configuration;

namespace Passbook.Generator.Configuration
{
	[ConfigurationCollection(typeof(LanguageElement), AddItemName = "language", CollectionType = ConfigurationElementCollectionType.BasicMap)]
	public class LanguageCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new LanguageElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((LanguageElement)element).Code;
		}
	}

	public class LanguageElement : ConfigurationElement
	{
		public LanguageElement()
		{
		}

		[ConfigurationProperty("code", DefaultValue = null, IsRequired = true, IsKey = true)]
		public String Code
		{
			get { return (String)this["code"]; }
			set { this["code"] = value; }
		}

		[ConfigurationProperty("", IsDefaultCollection = true)]
		public LocalizedEntryCollection Localizations
		{
			get { return (LocalizedEntryCollection)this[""]; }
			set	{ this[""] = value; }
		}
	}

	[ConfigurationCollection(typeof(LocalizedEntry), AddItemName = "entry", CollectionType = ConfigurationElementCollectionType.BasicMap)]
	public class LocalizedEntryCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new LocalizedEntry();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((LocalizedEntry)element).Key;
		}
	}

	public class LocalizedEntry : ConfigurationElement
	{
		public LocalizedEntry()
		{
		}

		[ConfigurationProperty("key", DefaultValue = null, IsRequired = true, IsKey = true)]
		public String Key
		{
			get { return (String)this["key"]; }
			set	{ this["key"] = value; }
		}

		[ConfigurationProperty("value", DefaultValue = null, IsRequired = true, IsKey = false)]
		public String Value
		{
			get { return (String)this["value"]; }
			set	{ this["value"] = value; }
		}
	}
}
