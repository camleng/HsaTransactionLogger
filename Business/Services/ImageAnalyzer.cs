using System;
using System.Threading.Tasks;
using Business.Models;
using Business.Validators;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace Business.Services
{
    public class ImageAnalyzer : IImageAnalyzer
    {
        private readonly IRestClient _client;
        private readonly CognitiveServicesConfig _config;
        private readonly ITaskProxy _task;

        public ImageAnalyzer(IRestClient client, IConfiguration configuration, ITaskProxy task)
        {
            _client = client;
            _task = task;
            _config = configuration.GetSection(nameof(CognitiveServicesConfig)).Get<CognitiveServicesConfig>();
            ConfigurationValidators.VerifyCognitiveServicesConfig(_config);
        }
        
        public async Task<string> GetAnalyzeResultAsync(string operationLocationAddress)
        {
            var analyzeResultRequest = CreateAnalyzeResultRestRequest(operationLocationAddress);
            var numberOfRetries = 0;
            while (true)
            {
                if (numberOfRetries > _config.MaxNumberOfRetries)
                {
                    throw new Exception("Max number of retries exceeded");
                }

                var analyzeResultResponse = _client.Get<HsaResult>(analyzeResultRequest);
                if (!analyzeResultResponse.IsSuccessful)
                {
                    throw new Exception(analyzeResultResponse.Content);
                }

                if (OperationSucceeded(analyzeResultResponse))
                {
                    return analyzeResultResponse.Content;
                }
                
                numberOfRetries++;
                await _task.Delay(CalculateRetryPeriod(numberOfRetries));
            }
        }

        private static int CalculateRetryPeriod(int numberOfRetries)
        {
            var delayMilliseconds = Math.Pow(2, numberOfRetries) * 1000;
            return Convert.ToInt32(delayMilliseconds);
        }

        private RestRequest CreateAnalyzeResultRestRequest(string operationLocationAddress)
        {
            var analyzeResultRequest = new RestRequest(operationLocationAddress);
            AddApiKeyHeader(analyzeResultRequest);
            return analyzeResultRequest;
        }
        
        private static bool OperationSucceeded(IRestResponse<HsaResult> analyzeResultResponse)
        {
            return analyzeResultResponse.Data.Status == "succeeded";
        }

        private void AddApiKeyHeader(IRestRequest request)
        {
            request.AddHeader(_config.ApiKey.Name, _config.ApiKey.Value);
        }
    }
}