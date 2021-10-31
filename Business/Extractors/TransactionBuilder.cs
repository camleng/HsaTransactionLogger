using System.Collections.Generic;
using System.Linq;
using Business.Models;

namespace Business.Extractors
{
    public static class TransactionBuilder
    {
        public static List<Transaction> BuildTransactions(HsaResult hsaResult)
        {
            var lines = TextLineExtractor.ExtractTextLines(hsaResult);

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