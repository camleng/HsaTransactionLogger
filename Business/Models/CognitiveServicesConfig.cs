namespace Business.Models;

public record CognitiveServicesConfig
{
    public string? BaseAddress { get; set; }
    public string? AnalyzeAddress { get; set; }
    public ApiKey? ApiKey { get; set; }
    public int? MaxNumberOfTries { get; set; }
}