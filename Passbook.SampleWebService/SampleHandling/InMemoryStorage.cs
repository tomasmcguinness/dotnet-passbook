using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Passbook.SampleWebService.SampleHandling
{
    public class InMemoryStorage
    {
        private Dictionary<string, InMemoryPassTypeStore> _passTypeStore;

        public InMemoryStorage()
        {
            _passTypeStore = new Dictionary<string, InMemoryPassTypeStore>();
        }

        internal bool IsPassRegistered(string passTypeIdentifier, string deviceLibraryIdentifier, string serialNumber)
        {
            return false;
        }

        internal void AddPass(string passTypeIdentifier, string deviceLibraryIdentifier, string serialNumber, string pushToken)
        {
            var store = StoreForPassTypeIdentifier(passTypeIdentifier);
            store.Add(deviceLibraryIdentifier, serialNumber, pushToken);
        }

        private InMemoryPassTypeStore StoreForPassTypeIdentifier(string passTypeIdentifier)
        {
            if (!_passTypeStore.ContainsKey(passTypeIdentifier))
            {
                _passTypeStore[passTypeIdentifier] = new InMemoryPassTypeStore();
            }

            return _passTypeStore[passTypeIdentifier];
        }

        internal void RemovePass(string passTypeIdentifier, string deviceLibraryIdentifier, string serialNumber)
        {
            throw new NotImplementedException();
        }
    }
}
