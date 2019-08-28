using System.Threading.Tasks;

namespace Passbook.SampleWebService.Services
{
    public interface IPassService
    {
        Task<byte[]> GeneratePassAsync(string serialNumber, string value, string secret);
    }
}