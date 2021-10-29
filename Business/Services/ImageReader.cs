using Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace Business.Services;

public class ImageReader : IImageReader
{
    private readonly CognitiveServicesConfig _config = new();

    public ImageReader(IConfiguration configuration)
    {
        configuration.GetSection(nameof(CognitiveServicesConfig)).Bind(_config);
    }

    public async Task<string> ReadTextFromImageToJson(IFormFile file)
    {
        await using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var bytes = memoryStream.ToArray();

        var restRequest = new RestRequest(_config.AnalyzeAddress);
        restRequest.AddHeader(_config.ApiKey.Name, _config.ApiKey.Value);
        restRequest.AddFileBytes("file", bytes, "file", "image/png");
        var client = new RestClient(_config.BaseAddress);
        var postImageResponse = client.Post(restRequest);

        if (!postImageResponse.IsSuccessful)
        {
            throw new Exception(postImageResponse.Content);
        }

        var operationLocationAddress = postImageResponse.Headers.SingleOrDefault(h => h.Name == "Operation-Location")
            ?.Value?.ToString();
        if (operationLocationAddress is null)
        {
            throw new Exception("Response does not include Operation-Location header");
        }

        var analyzeResultRequest = new RestRequest(operationLocationAddress);
        analyzeResultRequest.AddHeader(_config.ApiKey.Name, _config.ApiKey.Value);

        var numberOfTries = 0;
        while (true)
        {
            if (numberOfTries > _config.MaxNumberOfTries) throw new Exception("Max number of retries exceeded");

            var analyzeResultResponse = client.Get<HsaResult>(analyzeResultRequest);

            if (!analyzeResultResponse.IsSuccessful) throw new Exception(analyzeResultResponse.Content);
            
            var foundResult = analyzeResultResponse.Data.Status switch
            {
                "running" => false,
                "succeeded" => true,
                _ => false
            };

            if (!foundResult)
            {
                numberOfTries++;
                await Task.Delay(2000);
                continue;
            }

            if (foundResult)
            {
                return analyzeResultResponse.Content;
            }
        }
    }
}