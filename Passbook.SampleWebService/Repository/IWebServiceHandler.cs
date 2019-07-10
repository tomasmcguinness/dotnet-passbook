using System.Threading.Tasks;

namespace Passbook.SampleWebService
{
    public interface IWebServiceHandler
    {
        Task<PassRegistrationResult> RegisterPassAsync(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber, string pushToken, string secret);

        Task<bool> IsAuthorizedAsync(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber, string authorizationToken);

        Task UnregisterPassAsync(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber);

        string[] GetSerialNumbersOfPassesUpdatedSince(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string tag);

        byte[] GetUpdatedPass(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber);
    }
}