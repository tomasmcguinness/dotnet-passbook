using System;
using System.Configuration;

namespace Passbook.Generator.Configuration
{
	[ConfigurationCollection(typeof(int), AddItemName = "storeIdentifier", CollectionType = ConfigurationElementCollectionType.BasicMap )]
	public class StoreIdentifierCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new ConfigurationProperty<int>();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ConfigurationProperty<int>)element).Value;
		}
	}
}

