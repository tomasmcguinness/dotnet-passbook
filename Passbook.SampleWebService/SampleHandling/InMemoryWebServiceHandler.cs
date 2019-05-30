using System;

namespace Passbook.SampleWebService.SampleHandling
{
    public class InMemoryWebServiceHandler : IWebServiceHandler
    {
        private InMemoryStorage _storage;

        public InMemoryWebServiceHandler()
        {
            _storage = new InMemoryStorage();
        }

        public PassRegistrationResult RegisterPass(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber, string pushToken)
        {
            if (_storage.IsPassRegistered(passTypeIdentifier, deviceLibraryIdentifier, serialNumber))
            {
                return PassRegistrationResult.AlreadyRegistered;
            }
            else
            {
                _storage.AddPass(passTypeIdentifier, deviceLibraryIdentifier, serialNumber, pushToken);
                return PassRegistrationResult.Registered;
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

        public void UnregisterPass(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber)
        {
            _storage.RemovePass(passTypeIdentifier, deviceLibraryIdentifier, serialNumber);
        }

        public bool IsAuthorized(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber, string authorizationToken)
        {
            return authorizationToken == "TESTTOKEN";
        }
    }
}
