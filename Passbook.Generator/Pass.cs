using System.IO;

namespace Passbook.Generator;

public class Pass(string packagePathAndName)
{
    private string packagePathAndName = packagePathAndName;

    public byte[] GetPackage()
    {
        byte[] contents = File.ReadAllBytes(packagePathAndName);
        return contents;
    }

    public string PackageDirectory
    {
        get { return Path.GetDirectoryName(packagePathAndName); }
    }
}
