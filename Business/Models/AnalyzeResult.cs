namespace Business.Models;

public record AnalyzeResult
{
    public List<ReadResult>? ReadResults { get; set; }
}