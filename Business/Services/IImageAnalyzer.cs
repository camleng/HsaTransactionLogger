using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RestSharp;

namespace Business.Services
{
    public interface IImageAnalyzer
    {
        Task<IRestResponse> AnalyzeAsync(IFormFile file);
    }
}