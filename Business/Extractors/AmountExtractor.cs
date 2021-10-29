using Business.Parsers;

namespace Business.Extractors;

public static class AmountExtractor
{
    public static List<decimal> ExtractAmountsFrom(IEnumerable<string> lines)
    {
        return lines.Select(l => AmountParser.Parse(l, out var amount).Success
                ? amount
                : 0)
            .Where(p => p != 0)
            .ToList();
    }
}