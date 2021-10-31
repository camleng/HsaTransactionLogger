using System.Threading.Tasks;

namespace Business.Services
{
    public interface IImageAnalyzer
    {
        Task<string> GetAnalyzeResultAsync(string operationLocationAddress);
    }
}