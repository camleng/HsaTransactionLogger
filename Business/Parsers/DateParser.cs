using System;
using Business.Models;

namespace Business.Parsers
{
    public static class DateParser
    {
        public static ParseResult Parse(string text, out DateTime date)
        {
            var success = DateTime.TryParse(text, out date);
            return new ParseResult(success);
        }
    }
}