using System;
using Passbook.Generator.Fields;
using System.Configuration;

namespace Passbook.Generator.Configuration
{
	public enum FieldType
	{
		Standard,
		Date,
		Number
	}

	[ConfigurationCollection(typeof(FieldElement), AddItemName = "field", CollectionType = ConfigurationElementCollectionType.BasicMap )]
	public class FieldCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new FieldElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((FieldElement)element).Key;
		}
	}

	public class FieldElement : ConfigurationElement
	{
		public FieldElement()
		{
		}

		/// <summary>
		/// Field type to use
		///	<list type="bullet">
		///		<item>
		///			<description>Standard</description>
		///		</item>
		///		<item>
		///			<description>Number</description>
		///		</item>
		///		<item>
		///			<description>Date</description>
		///		</item>
		///	</list>
		/// </summary>
		[ConfigurationProperty("type", DefaultValue = FieldType.Standard, IsRequired = true, IsKey = true)]
		public FieldType Type
		{
			get { return (FieldType)this["type"]; }
			set { this["type"] = value; }
		}

		/// <summary>
		/// Required. The key must be unique within the scope of the entire pass. For example, "departure-gate".
		/// </summary>
		[ConfigurationProperty("key", DefaultValue = null, IsRequired = true, IsKey = true)]
		public String Key
		{
			get { return (String)this["key"]; }
			set { this["key"] = value; }
		}

		/// <summary>
		/// Optional. Data detectors that are applied to the field’s value. Valid values are:
		///	<list type="bullet">
		///		<item>
		///			<description>PKDataDetectorTypePhoneNumber</description>
		///		</item>
		///		<item>
		///			<description>PKDataDetectorTypeLink</description>
		///		</item>
		///		<item>
		///			<description>PKDataDetectorTypeAddress</description>
		///		</item>
		///		<item>
		///			<description>PKDataDetectorTypeCalendarEvent</description>
		///		</item>
		///	</list>
		/// <para>The default value is all data detectors. Provide an empty array to use no data detectors.</para>
		/// <para>Data detectors are applied only to back fields.</para>
		/// </summary>
		[ConfigurationProperty("dataDetectorTypes", DefaultValue = DataDetectorTypes.PKDataDetectorAll, IsRequired = false, IsKey = false)]
		public DataDetectorTypes DataDetectorTypes
		{
			get { return (DataDetectorTypes)this["dataDetectorTypes"]; }
			set { this["dataDetectorTypes"] = value; }
		}

		/// <summary>
		/// <para>Optional. Alignment for the field’s contents. Must be one of the following values:</para>
		///	<list type="bullet">
		///		<item>
		///			<description>PKTextAlignmentLeft</description>
		///		</item>
		///		<item>
		///			<description>PKTextAlignmentCenter</description>
		///		</item>
		///		<item>
		///			<description>PKTextAlignmentRight</description>
		///		</item>
		///		<item>
		///			<description>PKTextAlignmentNatural</description>
		///		</item>
		///	</list>
		/// <para>The default value is natural alignment, which aligns the text appropriately based on its script direction.</para>
		/// <para>This key is not allowed for primary fields or back fields.</para>
		/// </summary>
		[ConfigurationProperty("textAlignment", DefaultValue = FieldTextAlignment.Unspecified, IsRequired = false, IsKey = false)]
		public FieldTextAlignment TextAlignment
		{
			get { return (FieldTextAlignment)this["textAlignment"]; }
			set { this["textAlignment"] = value; }
		}

		/// <summary>
		/// Style of date to display
		/// </summary>
		[ConfigurationProperty("dateStyle", DefaultValue = FieldDateTimeStyle.Unspecified, IsRequired = false, IsKey = false)]
		public FieldDateTimeStyle DateStyle
		{
			get { return (FieldDateTimeStyle)this["dateStyle"]; }
			set { this["dateStyle"] = value; }
		}

		/// <summary>
		/// Style of time to display
		/// </summary>
		[ConfigurationProperty("timeStyle", DefaultValue = FieldDateTimeStyle.Unspecified, IsRequired = false, IsKey = false)]
		public FieldDateTimeStyle TimeStyle
		{
			get { return (FieldDateTimeStyle)this["timeStyle"]; }
			set { this["timeStyle"] = value; }
		}

		/// <summary>
		/// Style of number to display
		/// </summary>
		[ConfigurationProperty("numberStyle", DefaultValue = FieldNumberStyle.Unspecified, IsRequired = false, IsKey = false)]
		public FieldNumberStyle NumberStyle
		{
			get { return (FieldNumberStyle)this["numberStyle"]; }
			set { this["numberStyle"] = value; }
		}

		/// <summary>
		/// Optional. Attributed value of the field.
		/// The value may contain HTML markup for links. Only the <a> tag and its href attribute are supported. 
		/// For example, the following is key/value pair specifies a link with the text "Edit my profile":
		///	"attributedValue": "<a href='http://example.com/customers/123'>Edit my profile</a>"
		///	This key’s value overrides the text specified by the value key.
		///	Available in iOS 7.0.
		/// </summary>
		[ConfigurationProperty("attributedValue", DefaultValue = null, IsRequired = false, IsKey = false)]
		public ConfigurationProperty<String> AttributedValue
		{
			get { return (ConfigurationProperty<String>)this["attributedValue"]; }
			set	{ this["attributedValue"] = value; }
		}

		/// <summary>
		/// Optional. Label text for the field.
		/// </summary>
		[ConfigurationProperty("changeMessage", DefaultValue = null, IsRequired = false, IsKey = false)]
		public ConfigurationProperty<String> ChangeMessage
		{
			get { return (ConfigurationProperty<String>)this["changeMessage"]; }
			set	{ this["changeMessage"] = value; }
		}

		/// <summary>
		/// Optional. Label text for the field.
		/// </summary>
		[ConfigurationProperty("label", DefaultValue = null, IsRequired = false, IsKey = false)]
		public ConfigurationProperty<String> Label
		{
			get { return (ConfigurationProperty<String>)this["label"]; }
			set	{ this["label"] = value; }
		}

		/// <summary>
		/// Required. Value of the field.
		/// </summary>
		[ConfigurationProperty("value", DefaultValue = null, IsRequired = true, IsKey = false)]
		public ConfigurationProperty<String> Value
		{
			get { return (ConfigurationProperty<String>)this["value"]; }
			set	{ this["value"] = value; }
		}

		/// <summary>
		/// Optional. Always display the time and date in the given time zone, not in the user’s current time zone. The default value is false.
		/// The format for a date and time always requires a time zone, even if it will be ignored. For backward compatibility with iOS 6, provide an appropriate time zone, so that the information is displayed meaningfully even without ignoring time zones.
		///	This key does not affect how relevance is calculated.
		///	Available in iOS 7.0.
		/// </summary>
		[ConfigurationProperty("ignoresTimeZone", DefaultValue = null, IsRequired = true, IsKey = false)]
		public bool? IgnoresTimeZone
		{
			get 
			{
				bool value;

				if (bool.TryParse(this["ignoresTimeZone"] as String, out value))
					return value;

				return null;
			}
			set	
			{ 
				if (value.HasValue)
					this["ignoresTimeZone"] = value.Value.ToString();
				else
					this["ignoresTimeZone"] = null;
			}
		}

		/// <summary>
		/// Optional. If true, the label’s value is displayed as a relative date; otherwise, it is displayed as an absolute date. The default value is false.
		/// This does not affect how relevance is calculated.
		/// </summary>
		[ConfigurationProperty("isRelative", DefaultValue = null, IsRequired = true, IsKey = false)]
		public bool? IsRelative
		{
			get 
			{
				bool value;

				if (bool.TryParse(this["isRelative"] as String, out value))
					return value;

				return null;
			}
			set	
			{ 
				if (value.HasValue)
					this["isRelative"] = value.Value.ToString();
				else
					this["isRelative"] = null;
			}
		}

		/// <summary>
		/// ISO 4217 currency code for the field’s value.
		/// </summary>
		[ConfigurationProperty("currencyCode", DefaultValue = null, IsRequired = true, IsKey = false)]
		public ConfigurationProperty<String> CurrencyCode
		{
			get { return (ConfigurationProperty<String>)this["currencyCode"]; }
			set	{ this["currencyCode"] = value; }
		}
	}
}

