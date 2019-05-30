using System.Collections.Generic;

namespace Passbook.SampleWebService.SampleHandling
{
    public class InMemoryPassTypeStore
    {
        private List<RegisteredPass> _registeredPasses;

        public InMemoryPassTypeStore()
        {
            _registeredPasses = new List<RegisteredPass>();
        }

        internal void Add(string deviceLibraryIdentifier, string serialNumber, string pushToken)
        {
            _registeredPasses.Add(new RegisteredPass(deviceLibraryIdentifier, serialNumber, pushToken));
        }
    }
}
