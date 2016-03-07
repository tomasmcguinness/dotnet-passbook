using System;
using System.Configuration;

namespace Passbook.Generator.Configuration
{
	[ConfigurationCollection(typeof(ImageElement), AddItemName = "image", CollectionType = ConfigurationElementCollectionType.BasicMap )]
	public class ImageCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new ImageElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ImageElement)element).Type;
		}
	}

	public class ImageElement : ConfigurationElement
	{
		public ImageElement()
		{
		}

		[ConfigurationProperty("type", DefaultValue = null, IsRequired = true, IsKey = true)]
		public PassbookImage Type
		{
			get { return (PassbookImage)this["type"]; }
			set	{ this["type"] = value; }
		}

		[ConfigurationProperty("fileName", DefaultValue = null, IsRequired = true, IsKey = false)]
		public string FileName
		{
			get { return (string)this["fileName"]; }
			set	{ this["fileName"] = value; }
		}
	}
}

