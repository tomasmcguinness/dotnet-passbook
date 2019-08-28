using Microsoft.WindowsAzure.Storage.Table;

namespace Passbook.SampleWebService.Repository
{
    public class PassEntity : TableEntity
    {
        public PassEntity()
        {

        }

        public PassEntity(string partitionKey, string rowKey, string value, string secret)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            Value = value;
            Secret = secret;
        }

        public string Value { get; set; }

        public string Secret { get; set; }
    }
}
