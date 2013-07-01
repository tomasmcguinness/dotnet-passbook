using System;

namespace Passbook.Generator.Fields
{
    public class DateField : Field
    {
        public DateField()
            : base()
        { }

        public DateField(string key, string label, FieldDateTimeStyle dateStyle, FieldDateTimeStyle timeStyle)
            : base(key, label)
        {
            this.DateStyle = dateStyle;
            this.TimeStyle = timeStyle;
        }

        public DateField(string key, string label, FieldDateTimeStyle dateStyle, FieldDateTimeStyle timeStyle, DateTime value)
            : base(key, label)
        {
            this.Value = value;
            this.DateStyle = dateStyle;
            this.TimeStyle = timeStyle;
        }

        public DateTime Value { get; set; }
        public FieldDateTimeStyle DateStyle { get; set; }
        public FieldDateTimeStyle TimeStyle { get; set; }
        public bool IsRelative { get; set; }

        protected override void WriteKeys(Newtonsoft.Json.JsonWriter writer)
        {
            if (DateStyle != FieldDateTimeStyle.Unspecified)
            {
                writer.WritePropertyName("dateStyle");
                writer.WriteValue(DateStyle.ToString());
            }

            if (TimeStyle != FieldDateTimeStyle.Unspecified)
            {
                writer.WritePropertyName("timeStyle");
                writer.WriteValue(TimeStyle.ToString());
            }

            writer.WritePropertyName("isRelative");
            writer.WriteValue(IsRelative);
        }

        protected override void WriteValue(Newtonsoft.Json.JsonWriter writer)
        {
            writer.WriteValue(Value.ToString("yyyy-MM-ddTHH:mmZ"));
        }

        public override void SetValue(object value)
        {
            this.Value = (DateTime)value;
        }

        public override bool HasValue
        {
            get { return true; }
        }
    }
}
