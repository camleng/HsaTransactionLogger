using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RestSharp;

namespace Business.Services
{
    public class ImageReader : IImageReader
    {
        private readonly IImageUploader _imageUploader;
        private readonly IImageAnalyzer _imageAnalyzer;

        public ImageReader(IImageUploader imageUploader, IImageAnalyzer imageAnalyzer)
        {
            _imageUploader = imageUploader;
            _imageAnalyzer = imageAnalyzer;
        }

        public async Task<string> ReadTextFromImageToJsonAsync(IFormFile file)
        {
            var postImageResponse = await _imageUploader.StartImageReadingProcessAsync(file);
            var operationLocationAddress = GetOperationLocationHeader(postImageResponse);
            return await _imageAnalyzer.GetAnalyzeResultAsync(operationLocationAddress);
        }

        private string GetOperationLocationHeader(IRestResponse postImageResponse)
        {
            var operationLocationAddress = postImageResponse.Headers
                .SingleOrDefault(h => h.Name == "Operation-Location")
                ?.Value?.ToString();
            if (operationLocationAddress is null)
            {
                throw new Exception("Response does not include Operation-Location header");
            }

            return operationLocationAddress;
        }
    }
}