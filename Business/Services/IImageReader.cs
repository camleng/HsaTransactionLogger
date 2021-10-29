using Microsoft.AspNetCore.Http;

public interface IImageReader
{
    Task<string> ReadTextFromImageToJson(IFormFile file);
}