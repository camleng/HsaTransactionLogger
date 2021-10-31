using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Business.Extractors
{
    public static class TransactionTypeExtractor
    {
        public static List<string> ExtractTransactionTypesFrom(IEnumerable<string> lines)
        {
            return lines
                .Where(line => Regex.IsMatch(line, @"(Deposit|Benefits Card)"))
                .ToList();
        }
    }
}