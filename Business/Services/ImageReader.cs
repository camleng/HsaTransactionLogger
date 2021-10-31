using System;
using System.Linq;
using System.Threading.Tasks;
using Business.Models;
using Microsoft.AspNetCore.Http;
using RestSharp;

namespace Business.Services
{
    public class ImageReader : IImageReader
    {
        private readonly IImageAnalyzer _imageAnalyzer;
        private readonly IImageAnalyzeResultReader _imageAnalyzeResultReader;

        public ImageReader(IImageAnalyzer imageAnalyzer, IImageAnalyzeResultReader imageAnalyzeResultReader)
        {
            _imageAnalyzer = imageAnalyzer;
            _imageAnalyzeResultReader = imageAnalyzeResultReader;
        }

        public async Task<HsaResult> GetHsaTextInformationFromImage(IFormFile file)
        {
            var analyzeResponse = await _imageAnalyzer.AnalyzeAsync(file);
            var operationLocationAddress = GetOperationLocationHeader(analyzeResponse);
            return await _imageAnalyzeResultReader.GetAnalyzeResultAsync(operationLocationAddress);
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