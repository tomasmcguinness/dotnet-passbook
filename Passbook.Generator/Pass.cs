using System.IO;

namespace Passbook.Generator
{
    public class Pass
    {
        private string packagePathAndName;

        public Pass(string packagePathAndName)
        {
            this.packagePathAndName = packagePathAndName;
        }

        public byte[] GetPackage()
        {
            byte[] contents = File.ReadAllBytes(packagePathAndName);
            return contents;
        }

        public string PackageDirectory
        {
            get { return Path.GetDirectoryName(this.packagePathAndName); }
        }
    }
}
