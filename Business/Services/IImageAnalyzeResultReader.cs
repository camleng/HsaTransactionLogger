using System.Threading.Tasks;
using Business.Models;

namespace Business.Services
{
    public interface IImageAnalyzeResultReader
    {
        Task<HsaResult> GetAnalyzeResultAsync(string operationLocationAddress);
    }
}