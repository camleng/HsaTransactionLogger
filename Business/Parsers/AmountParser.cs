using System.Text.RegularExpressions;
using Business.Models;

namespace Business.Parsers
{
    public static class AmountParser
    {
        private const string AmountPattern = @"\$?(\d+\.\d{2})";

        public static ParseResult Parse(string text, out decimal amount)
        {
            var regexMatch = Regex.Match(text, AmountPattern);
            if (!regexMatch.Success)
            {
                amount = 0;
                return new ParseResult(false);
            }

            var amountPortion = regexMatch.Groups[1].Value;
            var decimalMatch = decimal.TryParse(amountPortion, out amount);
            return new ParseResult(decimalMatch);
        }
    }
}