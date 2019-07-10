using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Passbook.SampleWebService.Repository
{
    public class PassEntity : TableEntity
    {
        public PassEntity()
        {

        }

        public PassEntity(string partitionKey, string rowKey, string pushToken, string secret)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            PushToken = pushToken;
            Secret = secret;
        }

        public string PushToken { get; set; }

        public string Secret { get; set; }

        public string Value { get; set; }

        public DateTime? LastUpdated { get; set; }
    }
}
