using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passbook.SampleWebService.Repository
{
    public class InMemoryHandler : IWebServiceHandler
    {
        private static readonly List<Pass> _passes = new List<Pass>();
        private static readonly List<PassRegistration> _registrations = new List<PassRegistration>();

        public Task CreatePass(string passTypeIdentifier, string serialNumber, string value, string secret)
        {
            var pass = new Pass(passTypeIdentifier, serialNumber, value, secret);
            SavePass(pass);
            return Task.CompletedTask;
        }

        public PassRegistrationResult RegisterPass(string version,
                                                   string deviceLibraryIdentifier,
                                                   string passTypeIdentifier,
                                                   string serialNumber,
                                                   string pushToken)
        {
            var existingPass = GetPassRegistration(deviceLibraryIdentifier, passTypeIdentifier, serialNumber);

            if (existingPass == null)
            {
                var newPass = new PassRegistration(deviceLibraryIdentifier, passTypeIdentifier, serialNumber, pushToken);

                StorePassRegistration(newPass);

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

        public void UnregisterPass(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber)
        {
            var pass = GetPassRegistration(deviceLibraryIdentifier, passTypeIdentifier, serialNumber);
            DeletePassRegistration(pass);
        }

        public bool IsAuthorized(string passTypeIdentifier,
                                 string serialNumber,
                                 string authorizationToken)
        {
            var pass = GetPass(passTypeIdentifier, serialNumber);
            return authorizationToken == pass.AuthenticationToken;
        }

        // Private methods for existing the passes
        //
        private void SavePass(Pass newPass)
        {
            _passes.Add(newPass);
        }

        private Pass GetPass(string passTypeIdentifier, string serialNumber)
        {
            var pass = _passes.Single(p => p.PassTypeIdentifier == passTypeIdentifier && p.SerialNumber == serialNumber);
            return pass;
        }

        private void StorePassRegistration(PassRegistration registration)
        {
            _registrations.Add(registration);
        }

        private void DeletePassRegistration(PassRegistration registration)
        {
            _registrations.Remove(registration);
        }

        private PassRegistration GetPassRegistration(string deviceLibraryIdentifier,
                                                     string passTypeIdentifier,
                                                     string serialNumber)
        {
            return _registrations.Single(r => r.PassTypeIdentifier == passTypeIdentifier &&
                                         r.DeviceLibraryIdentifier == deviceLibraryIdentifier &&
                                         r.SerialNumber == serialNumber);
        }
    }
}
