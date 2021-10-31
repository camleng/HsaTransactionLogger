using System.Collections.Generic;
using System.Linq;
using Business.Models;

namespace Business.Extractors
{
    public static class TransactionExtractor
    {
        public static List<Transaction> BuildTransactionsFromJson(string json)
        {
            var lines = TextLineExtractor.ExtractTextLinesFromJson(json);

            var dates = DateExtractor.ExtractDatesFrom(lines);
            var prices = AmountExtractor.ExtractAmountsFrom(lines);
            var transactionTypes = TransactionTypeExtractor.ExtractTransactionTypesFrom(lines);

            var datesAndPricesTuple = dates.Zip(prices);
            var transactionsTuple = datesAndPricesTuple.Zip(transactionTypes);

            return transactionsTuple.Select(t =>
                    new Transaction(t.First.First, t.First.Second, t.Second))
                .ToList();
        }
    }
}