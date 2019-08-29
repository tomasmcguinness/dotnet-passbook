namespace Passbook.SampleWebService.Repository
{
    public class Pass
    {
        public Pass(string passTypeIdentifier, string serialNumber, string value, string authenticationToken)
        {
            PassTypeIdentifier = passTypeIdentifier;
            SerialNumber = serialNumber;
            Value = value;
            AuthenticationToken = authenticationToken;
        }

        public string PassTypeIdentifier { get; set; }

        public string SerialNumber { get; set; }

        public string Value { get; set; }

        public string AuthenticationToken { get; set; }
    }
}
