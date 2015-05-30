using System;
using System.Configuration;

namespace Passbook.Generator.Configuration
{
	public sealed class PassbookGeneratorSection : ConfigurationSection
	{
		[ConfigurationProperty("appleWWDRCACertificate", DefaultValue = null, IsRequired = false, IsKey = false)]
		public string AppleWWDRCACertificate
		{
			get { return this["appleWWDRCACertificate"] as String; }
			set	{ this["appleWWDRCACertificate"] = value; }
		}

		[ConfigurationProperty("templates", IsDefaultCollection = false)]
		public TemplateCollection Templates
		{
			get { return (TemplateCollection)this["templates"]; }
			set { this ["templates"] = value; }
		}
	}
}

