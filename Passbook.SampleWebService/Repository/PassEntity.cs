using Microsoft.WindowsAzure.Storage.Table;

namespace Passbook.SampleWebService.Repository
{
    public class PassEntity : TableEntity
    {
        public PassEntity()
        {

        }

        public PassEntity(string partitionKey, string rowKey, string value, string authenticationToken)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            Value = value;
            AuthenticationToken = authenticationToken;
        }

        public string Value { get; set; }

        public string AuthenticationToken { get; set; }
    }
}
