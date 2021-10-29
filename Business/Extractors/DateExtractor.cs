using Business.Parsers;

namespace Business.Extractors;

public static class DateExtractor
{
    public static List<DateTime> ExtractDatesFrom(IEnumerable<string> lines)
    {
        return lines.Select(l => DateParser.Parse(l, out var date).Success
            ? date
            : DateTime.MinValue)
            .Where(d => d != DateTime.MinValue)
            .ToList();
    }
}