namespace Passbook.Generator
{
    public class Nfc
    {
        public Nfc()
        {
        }

        public Nfc(string message)
        {
            Message = message;
        }

        public Nfc(string message, string encryptionPublicKey) : this(message)
        {
            EncryptionPublicKey = encryptionPublicKey;
        }

        public string Message { get; set; }

        public string EncryptionPublicKey { get; set; }
    }
}
