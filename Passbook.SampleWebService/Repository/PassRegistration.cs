namespace Passbook.SampleWebService.Repository
{
    public class PassRegistration
    {
        public PassRegistration(string passTypeIdentifier, string deviceLibraryIdentifier, string serialNumber, string pushToken)
        {
            PassTypeIdentifier = passTypeIdentifier;
            DeviceLibraryIdentifier = deviceLibraryIdentifier;
            SerialNumber = serialNumber;
            PushToken = pushToken;
        }

        public string PassTypeIdentifier { get; set; }

        public string DeviceLibraryIdentifier { get; set; }

        public string SerialNumber { get; set; }

        public string PushToken { get; set; }
    }
}
