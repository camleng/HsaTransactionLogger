using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RestSharp;

namespace Business.Services
{
    public interface IImageUploader
    {
        Task<IRestResponse> StartImageReadingProcessAsync(IFormFile file);
    }
}