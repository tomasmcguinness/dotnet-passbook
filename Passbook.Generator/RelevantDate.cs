using Passbook.Generator.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Passbook.Generator
{
    public class RelevantDate
    {
        /// <summary>
        /// The date and time when the pass becomes relevant. Wallet automatically calculates a relevancy interval from this date.
        /// </summary>
        public DateTimeOffset? Date { get; set; }


        /// <summary>
        /// The date and time for the pass relevancy interval to end. Required when providing startDate.
        /// </summary>
        public DateTimeOffset? EndDate { get; set; }


        /// <summary>
        /// The date and time for the pass relevancy interval to begin.
        /// </summary>
        public DateTimeOffset? StartDate { get; set; }


        public void Write(Utf8JsonWriter writer)
        {
            Validate();

            writer.WriteStartObject();

            if (Date.HasValue)
            {
                writer.WritePropertyName("date");
                writer.WriteStringValue(Date.Value.ToString("yyyy-MM-ddTHH:mm:ssK"));
            }

            if (EndDate.HasValue)
            {
                writer.WritePropertyName("endDate");
                writer.WriteStringValue(EndDate.Value.ToString("yyyy-MM-ddTHH:mm:ssK"));
            }

            if (StartDate.HasValue)
            {
                writer.WritePropertyName("startDate");
                writer.WriteStringValue(StartDate.Value.ToString("yyyy-MM-ddTHH:mm:ssK"));
            }

            writer.WriteEndObject();
        }

        private void Validate()
        {
            if (StartDate.HasValue)
            {
                if(EndDate.HasValue == false)
                    throw new RequiredFieldValueMissingException("endDate");
            }
        }
    }
}
