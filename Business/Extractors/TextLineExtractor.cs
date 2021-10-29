using System.Text.Json;
using Business.Models;

namespace Business.Extractors;

public static class TextLineExtractor
{
    public static List<string> ExtractTextLinesFromJson(string json)
    {
        var hsaResult = JsonSerializer.Deserialize<HsaResult>(json,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        if (hsaResult is null)
        {
            Console.WriteLine("Hsa result was unable to be parsed");
            return new List<string>();
        }

        return hsaResult.AnalyzeResult.ReadResults
            .SelectMany(r => 
                r.Lines.Select(l => l.Text))
            .Reverse()
            .ToList();
    }
}