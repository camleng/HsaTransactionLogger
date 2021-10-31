using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Business.Services
{
    public interface IImageReader
    {
        Task<string> ReadTextFromImageToJsonAsync(IFormFile file);
    }
}