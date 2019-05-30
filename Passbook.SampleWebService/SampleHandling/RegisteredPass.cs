namespace Passbook.SampleWebService.SampleHandling
{
    public class RegisteredPass
    {
        public RegisteredPass(string deviceLibraryIdentifier, string serialNumber, string pushToken)
        {
            DeviceLibraryIdentifier = deviceLibraryIdentifier;
            SerialNumber = serialNumber;
            PushToken = pushToken;
        }

        public string DeviceLibraryIdentifier { get; }

        public string SerialNumber { get; }

        public string PushToken { get; }
    }
}
