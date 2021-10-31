using System.Threading.Tasks;
using Business.Models;
using Microsoft.AspNetCore.Http;

namespace Business.Services
{
    public interface IImageReader
    {
        Task<HsaResult> GetHsaTextInformationFromImage(IFormFile file);
    }
}