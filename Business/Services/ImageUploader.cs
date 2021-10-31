using System;
using System.IO;
using System.Threading.Tasks;
using Business.Models;
using Business.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace Business.Services
{
    public class ImageUploader : IImageUploader
    {
        private readonly IRestClient _client;
        private readonly CognitiveServicesConfig _config;

        public ImageUploader(IRestClient client, IConfiguration configuration)
        {
            _client = client;
            _config = configuration.GetSection(nameof(CognitiveServicesConfig)).Get<CognitiveServicesConfig>();
            ConfigurationValidators.VerifyCognitiveServicesConfig(_config);
        }

        public async Task<IRestResponse> StartImageReadingProcessAsync(IFormFile file)
        {
            var bytes = await GetBytesFromFile(file);

            var analyzeRestRequest = CreateAnalyzeRestRequest(bytes);
            var postImageResponse = _client.Post(analyzeRestRequest);

            if (!postImageResponse.IsSuccessful)
            {
                throw new Exception(postImageResponse.Content);
            }

            return postImageResponse;
        }

        private RestRequest CreateAnalyzeRestRequest(byte[] bytes)
        {
            var restRequest = new RestRequest(_config.AnalyzeAddress);
            AddApiKeyHeader(restRequest);
            restRequest.AddFileBytes("file", bytes, "file", "image/png");
            return restRequest;
        }

        private static async Task<byte[]> GetBytesFromFile(IFormFile file)
        {
            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var bytes = memoryStream.ToArray();
            return bytes;
        }

        private void AddApiKeyHeader(IRestRequest request)
        {
            request.AddHeader(_config.ApiKey.Name, _config.ApiKey.Value);
        }
    }
}