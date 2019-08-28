using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;

namespace Passbook.SampleWebService.Repository
{
    public class TableStorageHandler : IWebServiceHandler
    {
        private readonly TableStorageConfiguration _configuration;

        public TableStorageHandler(TableStorageConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task CreatePass(string passTypeIdentifier, string serialNumber, string value, string secret)
        {
            var pass = new PassEntity(passTypeIdentifier, serialNumber, value, secret);
            await InsertPassAsync(pass);
        }

        public async Task<PassRegistrationResult> RegisterPassAsync(string version,
                                                                    string deviceLibraryIdentifier,
                                                                    string passTypeIdentifier,
                                                                    string serialNumber,
                                                                    string pushToken)
        {
            var existingPass = await GetPassRegistrationAsync(deviceLibraryIdentifier, passTypeIdentifier, serialNumber);

            if (existingPass == null)
            {
                var newPass = new PassRegistrationEntity(GeneratePartitionKey(deviceLibraryIdentifier, passTypeIdentifier), serialNumber, pushToken);

                await InsertPassRegistrationAsync(newPass);

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
            var pass = await GetPassRegistrationAsync(deviceLibraryIdentifier, passTypeIdentifier, serialNumber);
            await DeletePassRegistrationAsync(pass);
        }

        public async Task<bool> IsAuthorizedAsync(string passTypeIdentifier,
                                                  string serialNumber,
                                                  string authorizationToken)
        {
            var pass = await GetPassAsync(passTypeIdentifier, serialNumber);
            return authorizationToken == pass.AuthenticationToken;
        }

        // Private methods for existing the passes
        //
        private async Task InsertPassAsync(PassEntity newPass)
        {
            var table = GetPassesTable();

            var operation = TableOperation.Insert(newPass);

            await table.ExecuteAsync(operation);
        }

        private async Task<PassEntity> GetPassAsync(string passTypeIdentifier, string serialNumber)
        {
            var table = GetPassesTable();

            var operation = TableOperation.Retrieve(passTypeIdentifier, serialNumber);

            var result = await table.ExecuteAsync(operation);

            return result.Result as PassEntity;
        }

        private async Task UpdatePassAsync(PassEntity pass)
        {
            var table = GetPassesTable();

            var operation = TableOperation.Replace(pass);

            await table.ExecuteAsync(operation);
        }

        private async Task InsertPassRegistrationAsync(PassRegistrationEntity registration)
        {
            var table = GetRegistrationsTable();

            var operation = TableOperation.Insert(registration);

            await table.ExecuteAsync(operation);
        }


        private async Task DeletePassRegistrationAsync(PassRegistrationEntity registration)
        {
            var table = GetRegistrationsTable();

            var operation = TableOperation.Delete(registration);

            await table.ExecuteAsync(operation);
        }

        private async Task<PassRegistrationEntity> GetPassRegistrationAsync(string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber)
        {
            var table = GetRegistrationsTable();

            var partitionKey = GeneratePartitionKey(deviceLibraryIdentifier, passTypeIdentifier);

            var operation = TableOperation.Retrieve(partitionKey, serialNumber);

            var result = await table.ExecuteAsync(operation);

            return result.Result as PassRegistrationEntity;
        }

        private string GeneratePartitionKey(string deviceLibraryIdentifier, string passTypeIdentifier)
        {
            return deviceLibraryIdentifier + "-" + passTypeIdentifier;
        }

        private CloudTable GetPassesTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_configuration.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("passes");

            return table;
        }

        private CloudTable GetRegistrationsTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_configuration.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("registrations");

            return table;
        }
    }
}
