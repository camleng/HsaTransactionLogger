using System;
using Business.Models;

namespace Business.Validators
{
    public static class ConfigurationValidators
    {
        public static void VerifyCognitiveServicesConfig(CognitiveServicesConfig config)
        {
            if (config.AnalyzeAddress is null)
                throw new Exception($"{nameof(CognitiveServicesConfig.AnalyzeAddress)} is not defined");
            if (config.BaseAddress is null)
                throw new Exception($"{nameof(CognitiveServicesConfig.BaseAddress)} is not defined");
            if (config.ApiKey?.Name is null)
                throw new Exception($"{nameof(CognitiveServicesConfig.ApiKey.Name)} is not defined");
            if (config.ApiKey?.Value is null)
                throw new Exception($"{nameof(CognitiveServicesConfig.ApiKey.Value)} is not defined");
            if (config.MaxNumberOfRetries is null)
                throw new Exception($"{nameof(CognitiveServicesConfig.MaxNumberOfRetries)} is not defined");
        }
    }
}