using System;
using System.Configuration;

namespace Passbook.Generator.Configuration
{
	public sealed class PassbookGeneratorSection : ConfigurationSection
	{
		[ConfigurationProperty("appleCertificate", DefaultValue = null, IsRequired = true, IsKey = false)]
		public string AppleCertificate
		{
			get { return this["appleCertificate"] as String; }
			set	{ this["appleCertificate"] = value; }
		}

		[ConfigurationProperty("templates", IsDefaultCollection = false)]
		public TemplateCollection Templates
		{
			get { return (TemplateCollection)this["templates"]; }
			set { this ["templates"] = value; }
		}
	}
}

