﻿using System.Text.Json;
using Passbook.Generator.Exceptions;

namespace Passbook.Generator;

public class RelevantBeacon
{
    /// <summary>
    /// Required. Unique identifier of a Bluetooth Low Energy location beacon.
    /// </summary>
    public string ProximityUUID { get; set; }

    /// <summary>
    /// Optional. Text displayed on the lock screen when the pass is currently relevant.
    /// </summary>
    public string RelevantText { get; set; }

    public int? Major { get; set; }

    public int? Minor { get; set; }

    public void Write(Utf8JsonWriter writer)
    {
        Validate();

        writer.WriteStartObject();

        writer.WritePropertyName("proximityUUID");
        writer.WriteStringValue(ProximityUUID);

        if (!string.IsNullOrEmpty(RelevantText))
        {
            writer.WritePropertyName("relevantText");
            writer.WriteStringValue(RelevantText);
        }

        if (Minor.HasValue)
        {
            writer.WritePropertyName("minor");
            writer.WriteNumberValue(Minor.Value);
        }

        if (Major.HasValue)
        {
            writer.WritePropertyName("major");
            writer.WriteNumberValue(Major.Value);
        }

        writer.WriteEndObject();
    }

    private void Validate()
    {
        if (string.IsNullOrEmpty(ProximityUUID))
        {
            throw new RequiredFieldValueMissingException("ProximityUUID");
        }
    }
}
