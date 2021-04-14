using Newtonsoft.Json;
using Passbook.Generator.Exceptions;
using System;

namespace Passbook.Generator.Fields
{
    public abstract class Field
    {
        public Field()
        {
            this.DataDetectorTypes = DataDetectorTypes.PKDataDetectorAll;
        }

        public Field(string key, string label)
            : this()
        {
            this.Key = key;
            this.Label = label;
        }

        public Field(string key, string label, string changeMessage, FieldTextAlignment textAligment)
            : this(key, label)
        {
            this.ChangeMessage = changeMessage;
            this.TextAlignment = textAligment;
        }

        /// <summary>
        /// Required. The key must be unique within the scope of the entire pass. For example, “departure-gate”.
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Optional. Label text for the field.
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// <para>Optional. Format string for the alert text that is displayed when the pass is updated.</para>
        /// <para>The format string must contain the escape %@, which is replaced with the field's new value.</para>
        /// <para>For example, "Gate changed to %@."</para>
        /// <para>If you don't specify a change message, the user isn't notified when the field changes.</para>
        /// </summary>
        public string ChangeMessage { get; set; }
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
        public FieldTextAlignment TextAlignment { get; set; }
        /// <summary>
        /// <para>Optional. Attributed value of the field.</para>
        /// <para>The value may contain HTML markup for links. Only the &lt;a&gt; tag and its href attribute are supported. For example, the following is key/value pair specifies a link with the text "Edit my profile":</para>
        /// <c>"attributedValue": "&lt;a href='http://example.com/customers/123&gt;>Edit my profile&lt;/a&gt;"</c>
        /// <para>This key's value overrides the text specified by the value key.</para>
        /// <para>Available in iOS 7.0.</para>
        /// </summary>
        public string AttributedValue { get; set; }

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
        public DataDetectorTypes DataDetectorTypes { get; set; }

        /// <summary>
        /// Optional for Auxiliary fields
        /// </summary>
        public int? Row { get; set; }

        public void Write(JsonWriter writer)
        {
            Validate();

            writer.WriteStartObject();

            writer.WritePropertyName("key");
            writer.WriteValue(Key);

            if (!string.IsNullOrEmpty(ChangeMessage))
            {
                writer.WritePropertyName("changeMessage");
                writer.WriteValue(ChangeMessage);
            }

            if (!string.IsNullOrEmpty(Label))
            {
                writer.WritePropertyName("label");
                writer.WriteValue(Label);
            }

            if (TextAlignment != FieldTextAlignment.Unspecified)
            {
                writer.WritePropertyName("textAlignment");
                writer.WriteValue(TextAlignment.ToString());
            }

            if (!string.IsNullOrEmpty(AttributedValue))
            {
                writer.WritePropertyName("attributedValue");
                writer.WriteValue(this.AttributedValue);
            }

            if (Row.HasValue)
            {
                writer.WritePropertyName("row");
                writer.WriteValue(this.Row.Value);
            }

            WriteKeys(writer);

            WriteDataDetectorTypes(writer);

            writer.WritePropertyName("value");
            WriteValue(writer);

            writer.WriteEndObject();
        }

        private void WriteDataDetectorTypes(JsonWriter writer)
        {
            if (DataDetectorTypes != DataDetectorTypes.PKDataDetectorAll)
            {
                writer.WritePropertyName("dataDetectorTypes");
                writer.WriteStartArray();

                foreach (Enum value in Enum.GetValues(typeof(DataDetectorTypes)))
                    if (value.CompareTo(DataDetectorTypes.PKDataDetectorNone) != 0 &&
                        value.CompareTo(DataDetectorTypes.PKDataDetectorAll) != 0 &&
                        DataDetectorTypes.HasFlag(value))
                        writer.WriteValue(value.ToString());

                writer.WriteEndArray();
            }
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(Key))
            {
                throw new RequiredFieldValueMissingException("key");
            }
        }

        protected virtual void WriteKeys(JsonWriter writer)
        {
            // NO OP
        }

        protected abstract void WriteValue(JsonWriter writer);

        public abstract void SetValue(object value);

        public abstract bool HasValue { get; }
    }
}
