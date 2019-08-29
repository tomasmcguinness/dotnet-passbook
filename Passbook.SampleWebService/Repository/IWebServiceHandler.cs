using System.Threading.Tasks;

namespace Passbook.SampleWebService.Repository
{
    public interface IWebServiceHandler
    {
        PassRegistrationResult RegisterPass(string version,
                                            string deviceLibraryIdentifier,
                                            string passTypeIdentifier,
                                            string serialNumber,
                                            string pushToken);

        bool IsAuthorized(string passTypeIdentifier,
                          string serialNumber,
                          string authorizationToken);

        void UnregisterPass(string version,
                            string deviceLibraryIdentifier,
                            string passTypeIdentifier,
                            string serialNumber);

        string[] GetSerialNumbersOfPassesUpdatedSince(string version,
                                                      string deviceLibraryIdentifier,
                                                      string passTypeIdentifier,
                                                      string tag);

        byte[] GetUpdatedPass(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber);
    }
}