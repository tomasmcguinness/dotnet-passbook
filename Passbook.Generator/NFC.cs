namespace Passbook.Generator;

public class Nfc(string message, string encryptionPublicKey)
{
    public string Message { get; } = message;

    public string EncryptionPublicKey { get; } = encryptionPublicKey;
}
