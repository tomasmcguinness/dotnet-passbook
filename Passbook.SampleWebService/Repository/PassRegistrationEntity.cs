using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Passbook.SampleWebService.Repository
{
    public class PassRegistrationEntity : TableEntity
    {
        public PassRegistrationEntity()
        {

        }

        public PassRegistrationEntity(string partitionKey, string rowKey, string pushToken)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            PushToken = pushToken;
        }

        public string PushToken { get; set; }
    }
}
