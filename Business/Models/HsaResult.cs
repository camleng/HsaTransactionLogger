namespace Business.Models;

public record HsaResult
{
    public string? Status { get; set; }
    public AnalyzeResult? AnalyzeResult { get; set; }
}