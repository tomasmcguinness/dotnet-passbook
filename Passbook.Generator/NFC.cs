namespace Passbook.Generator
{
    public class Nfc
    {
        public Nfc()
        {
        }

        public Nfc(string message)
        {
            this.Message = message;
        }

        public string Message { get; set; }
    }
}
