namespace Passbook.Generator
{
    public class Nfc
    {
        public Nfc(string message, string encryptionPublicKey)
        {
            Message = message;
            EncryptionPublicKey = encryptionPublicKey;
        }

        public string Message { get; }

        public string EncryptionPublicKey { get; }
    }
}
