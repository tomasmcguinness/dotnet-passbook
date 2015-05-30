using System;
using System.Configuration;

namespace Passbook.Generator.Configuration
{
	[ConfigurationCollection(typeof(TemplateElement), AddItemName = "template", CollectionType = ConfigurationElementCollectionType.BasicMap )]
	public class TemplateCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new TemplateElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((TemplateElement)element).Name;
		}
	}

	public class TemplateElement : ConfigurationElement
	{
		public TemplateElement()
		{
		}

		[ConfigurationProperty("name", DefaultValue = null, IsRequired = true, IsKey = true)]
		public string Name
		{
			get { return (string)this["name"]; }
			set	{ this["name"] = value; }
		}

		[ConfigurationProperty("passStyle", DefaultValue = PassStyle.Generic, IsRequired = true, IsKey = false)]
		public PassStyle PassStyle
		{
			get { return (PassStyle)this["passStyle"]; }
			set	{ this["passStyle"] = value; }
		}

		/// <summary>
		/// Required for boarding passes; otherwise not allowed. Type of transit. 
		/// Must be one of the following values: PKTransitTypeAir, PKTransitTypeBoat, PKTransitTypeBus, PKTransitTypeGeneric,PKTransitTypeTrain.
		/// </summary>
		[ConfigurationProperty("transitType", DefaultValue = TransitType.PKTransitTypeGeneric, IsRequired = false, IsKey = false)]
		public TransitType TransitType
		{
			get { return (TransitType)this["transitType"]; }
			set	{ this["transitType"] = value; }
		}

		#region Certificate Keys
		[ConfigurationProperty("certificate", DefaultValue = null, IsRequired = false, IsKey = false)]
		public string Certificate
		{
			get { return (string)this["certificate"]; }
			set	{ this["certificate"] = value; }
		}

		[ConfigurationProperty("certificatePassword", DefaultValue = null, IsRequired = false, IsKey = false)]
		public string CertificatePassword
		{
			get { return (string)this["certificatePassword"]; }
			set	{ this["certificatePassword"] = value; }
		}

        [ConfigurationProperty("certificateThumbprint", DefaultValue = null, IsRequired = false, IsKey = false)]
        public string CertificateThumbprint
        {
            get { return (string)this["certificateThumbprint"]; }
            set { this["certificateThumbprint"] = value; }
        }
		#endregion

		#region Standard Keys
		/// <summary>
		/// Required. Brief description of the pass, used by the iOS accessibility technologies.
		/// Don’t try to include all of the data on the pass in its description, just include enough detail to distinguish passes of the same type.
		/// </summary>
		[ConfigurationProperty("description", DefaultValue = null, IsRequired = true, IsKey = false)]
		public ConfigurationProperty<String> Description
		{
			get { return (ConfigurationProperty<String>)this["description"]; }
			set	{ this["description"] = value; }
		}

		/// <summary>
		/// Required. Display name of the organization that originated and signed the pass.
		/// </summary>
		/// <value>The name of the organization.</value>
		[ConfigurationProperty("organizationName", DefaultValue = null, IsRequired = true, IsKey = false)]
		public ConfigurationProperty<String> OrganizationName
		{
			get { return (ConfigurationProperty<String>)this["organizationName"]; }
			set	{ this["organizationName"] = value; }
		}

		/// <summary>
		/// Required. Pass type identifier, as issued by Apple. The value must correspond with your signing certificate.
		/// </summary>
		[ConfigurationProperty("passTypeIdentifier", DefaultValue = null, IsRequired = true, IsKey = false)]
		public ConfigurationProperty<String> PassTypeIdentifier
		{
			get { return (ConfigurationProperty<String>)this["passTypeIdentifier"]; }
			set	{ this["passTypeIdentifier"] = value; }
		}

		/// <summary>
		/// Required. Team identifier of the organization that originated and signed the pass, as issued by Apple.
		/// </summary>
		[ConfigurationProperty("teamIdentifier", DefaultValue = null, IsRequired = true, IsKey = false)]
		public ConfigurationProperty<String> TeamIdentifier
		{
			get { return (ConfigurationProperty<String>)this["teamIdentifier"]; }
			set	{ this["teamIdentifier"] = value; }
		}
		#endregion

		#region Associated App Keys
		/// <summary>
		/// Optional. A URL to be passed to the associated app when launching it.
		/// The app receives this URL in the application:didFinishLaunchingWithOptions: and application:handleOpenURL: methods of its app delegate.
		/// If this key is present, the associatedStoreIdentifiers key must also be present.
		/// </summary>
		[ConfigurationProperty("appLaunchURL", DefaultValue = null, IsRequired = false, IsKey = false)]
		public ConfigurationProperty<String> AppLaunchURL
		{
			get { return (ConfigurationProperty<String>)this["appLaunchURL"]; }
			set	{ this["appLaunchURL"] = value;	}
		}

		/// <summary>
		/// Optional. A list of iTunes Store item identifiers for the associated apps.
		/// Only one item in the list is used—the first item identifier for an app compatible with the current device. If the app is not installed, the link opens the App Store and shows the app. If the app is already installed, the link launches the app.
		/// </summary>
		[ConfigurationProperty("associatedStoreIdentifiers", IsDefaultCollection = false)]
		public StoreIdentifierCollection AssociatedStoreIdentifiers
		{
			get { return (StoreIdentifierCollection)this["associatedStoreIdentifiers"]; }
			set { this ["associatedStoreIdentifiers"] = value; }
		}
		#endregion

		#region Visual Appearance Keys
		/// <summary>
		/// Optional. Background color of the pass, specified as an CSS-style RGB triple. For example, rgb(23, 187, 82).
		/// </summary>
		[ConfigurationProperty("backgroundColor", DefaultValue = null, IsRequired = false, IsKey = false)]
		public ConfigurationProperty<String> BackgroundColor
		{
			get { return (ConfigurationProperty<String>)this["backgroundColor"]; }
			set	{ this["backgroundColor"] = value; }
		}

		/// <summary>
		/// Optional. Background color of the pass, specified as an CSS-style RGB triple. For example, rgb(23, 187, 82).
		/// </summary>
		[ConfigurationProperty("foregroundColor", DefaultValue = null, IsRequired = false, IsKey = false)]
		public ConfigurationProperty<String> ForegroundColor
		{
			get { return (ConfigurationProperty<String>)this["foregroundColor"]; }
			set	{ this["foregroundColor"] = value; }
		}

		/// <summary>
		/// Optional for event tickets and boarding passes; otherwise not allowed. Identifier used to group related passes. 
		/// If a grouping identifier is specified, passes with the same style, pass type identifier, and grouping identifier are displayed as a group. 
		/// Otherwise, passes are grouped automatically.
		/// Use this to group passes that are tightly related, such as the boarding passes for different connections of the same trip.
		///	Available in iOS 7.0.
		/// </summary>
		[ConfigurationProperty("groupingIdentifier", DefaultValue = null, IsRequired = false, IsKey = false)]
		public ConfigurationProperty<String> GroupingIdentifier
		{
			get { return (ConfigurationProperty<String>)this["groupingIdentifier"]; }
			set	{ this["groupingIdentifier"] = value; }
		}

		/// <summary>
		/// Optional. Color of the label text, specified as a CSS-style RGB triple. For example, rgb(255, 255, 255).
		/// If omitted, the label color is determined automatically.
		/// </summary>
		[ConfigurationProperty("labelColor", DefaultValue = null, IsRequired = false, IsKey = false)]
		public ConfigurationProperty<String> LabelColor
		{
			get { return (ConfigurationProperty<String>)this["labelColor"]; }
			set	{ this["labelColor"] = value; }
		}

		/// <summary>
		/// Optional. Text displayed next to the logo on the pass.
		/// </summary>
		[ConfigurationProperty("logoText", DefaultValue = null, IsRequired = false, IsKey = false)]
		public ConfigurationProperty<String> LogoText
		{
			get { return (ConfigurationProperty<String>)this["logoText"]; }
			set	{ this["logoText"] = value; }
		}

		/// <summary>
		/// Optional. If true, the strip image is displayed without a shine effect. The default value prior to iOS 7.0 is false.
		/// In iOS 7.0, a shine effect is never applied, and this key is deprecated.
		/// </summary>
		[ConfigurationProperty("suppressStripShine", DefaultValue = null, IsRequired = false, IsKey = false)]
		public ConfigurationProperty<bool> SuppressStripShine
		{
			get { return (ConfigurationProperty<bool>)this["suppressStripShine"]; }
			set	{ this["suppressStripShine"] = value; }
		}
		#endregion

		#region Web Service Keys
		/// <summary>
		/// The authentication token to use with the web service. The token must be 16 characters or longer.
		/// </summary>
		[ConfigurationProperty("authenticationToken", DefaultValue = null, IsRequired = false, IsKey = false)]
		public ConfigurationProperty<String> AuthenticationToken
		{
			get { return (ConfigurationProperty<String>)this["authenticationToken"]; }
			set	{ this["authenticationToken"] = value; }
		}
			
		/// <summary>
		/// The URL of a web service that conforms to the API described in Passbook Web Service Reference.
		/// The web service must use the HTTPS protocol; the leading https:// is included in the value of this key.
		/// On devices configured for development, there is UI in Settings to allow HTTP web services.
		/// </summary>
		[ConfigurationProperty("webServiceURL", DefaultValue = null, IsRequired = false, IsKey = false)]
		public ConfigurationProperty<String> WebServiceURL
		{
			get { return (ConfigurationProperty<String>)this["webServiceURL"]; }
			set	{ this["webServiceURL"] = value; }
		}
		#endregion

		#region Field Definitions
		/// <summary>
		/// Optional. Additional fields to be displayed on the front of the pass.
		/// </summary>
		[ConfigurationProperty("auxiliaryFields", IsDefaultCollection = false)]
		public FieldCollection AuxiliaryFields
		{
			get { return (FieldCollection)this["auxiliaryFields"]; }
			set { this ["auxiliaryFields"] = value; }
		}

		/// <summary>
		/// Optional. Fields to be on the back of the pass.
		/// </summary>
		[ConfigurationProperty("backFields", IsDefaultCollection = false)]
		public FieldCollection BackFields
		{
			get { return (FieldCollection)this["backFields"]; }
			set { this ["backFields"] = value; }
		}

		/// <summary>
		/// Optional. Fields to be displayed in the header on the front of the pass.
		/// Use header fields sparingly; unlike all other fields, they remain visible when a stack of passes are displayed.
		/// </summary>
		[ConfigurationProperty("headerFields", IsDefaultCollection = false)]
		public FieldCollection HeaderFields
		{
			get { return (FieldCollection)this["headerFields"]; }
			set { this ["headerFields"] = value; }
		}

		/// <summary>
		/// Optional. Fields to be displayed prominently on the front of the pass.
		/// </summary>
		[ConfigurationProperty("primaryFields", IsDefaultCollection = false)]
		public FieldCollection PrimaryFields
		{
			get { return (FieldCollection)this["primaryFields"]; }
			set { this ["primaryFields"] = value; }
		}

		/// <summary>
		/// Optional. Fields to be displayed on the front of the pass.
		/// </summary>
		[ConfigurationProperty("secondaryFields", IsDefaultCollection = false)]
		public FieldCollection SecondaryFields
		{
			get { return (FieldCollection)this["secondaryFields"]; }
			set { this ["secondaryFields"] = value; }
		}
		#endregion

		#region Images
		[ConfigurationProperty("images", IsDefaultCollection = false)]
		public ImageCollection Images
		{
			get { return (ImageCollection)this["images"]; }
			set { this ["images"] = value; }
		}
		#endregion

		#region Localizations
		[ConfigurationProperty("localizations", IsDefaultCollection = false)]
		public LanguageCollection Localizations
		{
			get { return (LanguageCollection)this["localizations"]; }
			set { this["localizations"] = value; }
		}
		#endregion
	}
}

