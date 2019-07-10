using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;

namespace Passbook.SampleWebService.Repository
{
    public class TableStorageWebServiceHandler : IWebServiceHandler
    {
        private readonly TableStorageConfiguration _configuration;

        public TableStorageWebServiceHandler(TableStorageConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<PassRegistrationResult> RegisterPassAsync(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber, string pushToken, string secret)
        {
            var existingPass = await GetPassAsync(deviceLibraryIdentifier, passTypeIdentifier, serialNumber);

            if (existingPass == null)
            {
                var newPass = new PassEntity(GeneratePartitionKey(deviceLibraryIdentifier, passTypeIdentifier), serialNumber, pushToken, secret);

                await InsertPassAsync(newPass);

                return PassRegistrationResult.Registered;
            }
            else
            {
                return PassRegistrationResult.AlreadyRegistered;
            }
        }

        public string[] GetSerialNumbersOfPassesUpdatedSince(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string tag)
        {
            throw new NotImplementedException();
        }

        public byte[] GetUpdatedPass(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber)
        {
            throw new NotImplementedException();
        }

        public async Task UnregisterPassAsync(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber)
        {
            var pass = await GetPassAsync(deviceLibraryIdentifier, passTypeIdentifier, serialNumber);
            await DeletePassAsync(pass);
        }

        public async Task<bool> IsAuthorizedAsync(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber, string authorizationToken)
        {
            var pass = await GetPassAsync(deviceLibraryIdentifier, passTypeIdentifier, serialNumber);
            return authorizationToken == pass.Secret;
        }

        // Private methods for existing the passes
        //
        private async Task InsertPassAsync(PassEntity newPass)
        {
            var table = GetTable();

            var operation = TableOperation.Insert(newPass);

            await table.ExecuteAsync(operation);
        }

        private async Task DeletePassAsync(PassEntity newPass)
        {
            var table = GetTable();

            var operation = TableOperation.Delete(newPass);

            await table.ExecuteAsync(operation);
        }

        private async Task UpdatePassAsync(PassEntity newPass)
        {
            var table = GetTable();

            var operation = TableOperation.Replace(newPass);

            await table.ExecuteAsync(operation);
        }

        private async Task<PassEntity> GetPassAsync(string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber)
        {
            var table = GetTable();

            var partitionKey = GeneratePartitionKey(deviceLibraryIdentifier, passTypeIdentifier);

            var operation = TableOperation.Retrieve(partitionKey, serialNumber);

            var result = await table.ExecuteAsync(operation);

            return result.Result as PassEntity;
        }

        private string GeneratePartitionKey(string deviceLibraryIdentifier, string passTypeIdentifier)
        {
            return deviceLibraryIdentifier + "-" + passTypeIdentifier;
        }

        private CloudTable GetTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_configuration.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("passes");

            return table;
        }
    }
}
