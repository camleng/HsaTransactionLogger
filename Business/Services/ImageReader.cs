using Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace Business.Services;

public class ImageReader : IImageReader
{
    private readonly CognitiveServicesConfig _config = new();
    private readonly IRestClient _client;

    public ImageReader(IConfiguration configuration, IRestClient client)
    {
        configuration.GetSection(nameof(CognitiveServicesConfig)).Bind(_config);
        VerifyConfig(_config);
        _client = client;
    }

    public async Task<string> ReadTextFromImageToJson(IFormFile file)
    {
        var postImageResponse = await StartImageReadingProcess(file);
        var operationLocationAddress = GetOperationLocationHeader(postImageResponse);
        return await GetAnalyzeResult(operationLocationAddress);
    }

    private async Task<IRestResponse> StartImageReadingProcess(IFormFile file)
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

    private async Task<string> GetAnalyzeResult(string operationLocationAddress)
    {
        var analyzeResultRequest = CreateAnalyzeResultRestRequest(operationLocationAddress);
        var numberOfTries = 0;
        while (true)
        {
            if (numberOfTries > _config.MaxNumberOfTries) throw new Exception("Max number of retries exceeded");

            var analyzeResultResponse = _client.Get<HsaResult>(analyzeResultRequest);
            if (!analyzeResultResponse.IsSuccessful) throw new Exception(analyzeResultResponse.Content);

            if (!OperationSucceeded(analyzeResultResponse))
            {
                numberOfTries++;
                await Task.Delay(2000);
                continue;
            }

            return analyzeResultResponse.Content;
        }
    }

    private string GetOperationLocationHeader(IRestResponse postImageResponse)
    {
        var operationLocationAddress = postImageResponse.Headers.SingleOrDefault(h => h.Name == "Operation-Location")
            ?.Value?.ToString();
        if (operationLocationAddress is null)
        {
            throw new Exception("Response does not include Operation-Location header");
        }

        return operationLocationAddress;
    }

    private RestRequest CreateAnalyzeRestRequest(byte[] bytes)
    {
        var restRequest = new RestRequest(_config.AnalyzeAddress);
        AddApiKeyHeader(restRequest);
        restRequest.AddFileBytes("file", bytes, "file", "image/png");
        return restRequest;
    }

    private RestRequest CreateAnalyzeResultRestRequest(string operationLocationAddress)
    {
        var analyzeResultRequest = new RestRequest(operationLocationAddress);
        AddApiKeyHeader(analyzeResultRequest);
        return analyzeResultRequest;
    }

    private void AddApiKeyHeader(IRestRequest request)
    {
        request.AddHeader(_config.ApiKey.Name, _config.ApiKey.Value);
    }

    private static async Task<byte[]> GetBytesFromFile(IFormFile file)
    {
        await using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var bytes = memoryStream.ToArray();
        return bytes;
    }

    private static bool OperationSucceeded(IRestResponse<HsaResult> analyzeResultResponse)
    {
        return analyzeResultResponse.Data.Status == "succeeded";
    }

    private void VerifyConfig(CognitiveServicesConfig config)
    {
        if (config.AnalyzeAddress is null) throw new Exception($"{nameof(CognitiveServicesConfig.AnalyzeAddress)} is not defined");
        if (config.BaseAddress is null) throw new Exception($"{nameof(CognitiveServicesConfig.BaseAddress)} is not defined");
        if (config.ApiKey?.Name is null) throw new Exception($"{nameof(CognitiveServicesConfig.ApiKey.Name)} is not defined");
        if (config.ApiKey?.Value is null) throw new Exception($"{nameof(CognitiveServicesConfig.ApiKey.Value)} is not defined");
    }
}