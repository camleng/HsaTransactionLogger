using System.Collections.Generic;
using System.Linq;
using Business.Models;

namespace Business.Extractors
{
    public static class TextLineExtractor
    {
        public static List<string> ExtractTextLines(HsaResult hsaResult)
        {
            return hsaResult.AnalyzeResult.ReadResults
                .SelectMany(r =>
                    r.Lines.Select(l => l.Text))
                .Reverse()
                .ToList();
        }
    }
}