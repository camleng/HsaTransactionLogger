namespace Business.Models;

public class CognitiveServicesConfig
{
    public string BaseAddress { get; set; }
    public string AnalyzeAddress { get; set; }
    public ApiKey ApiKey { get; set; }
    public int MaxNumberOfTries { get; set; }
}