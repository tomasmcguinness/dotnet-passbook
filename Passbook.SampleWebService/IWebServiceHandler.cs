namespace Passbook.SampleWebService
{
    public interface IWebServiceHandler
    {
        bool IsAuthorized(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber, string authorizationToken);

        PassRegistrationResult RegisterPass(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber, string pushToken);

        void UnregisterPass(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber);

        string[] GetSerialNumbersOfPassesUpdatedSince(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string tag);

        byte[] GetUpdatedPass(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber);
    }
}