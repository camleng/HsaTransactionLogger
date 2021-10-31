using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Business.Tests.TestHelpers
{
    public static class ConfigurationHelper
    {
        public static IConfigurationRoot CreateConfiguration(List<KeyValuePair<string, string>>? overrides = null)
        {
            var kvp = new List<KeyValuePair<string, string>>
            {
                new("CognitiveServicesConfig:AnalyzeAddress", "AnalyzeAddressValue"),
                new("CognitiveServicesConfig:BaseAddress", "BaseAddressValue"),
                new("CognitiveServicesConfig:ApiKey:Name", "ApiKeyName"),
                new("CognitiveServicesConfig:ApiKey:Value", "ApiKeyValue"),
                new("CognitiveServicesConfig:MaxNumberOfRetries", "5")
            };

            if (overrides is not null)
            {
                foreach (var overrideKvp in overrides)
                {
                    var index = kvp.FindIndex(p => p.Key == overrideKvp.Key);
                    if (index >= 0)
                    {
                        kvp[index]= overrideKvp;
                    }
                }
            }

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(kvp)
                .Build();
            return configuration;
        }
    }
}