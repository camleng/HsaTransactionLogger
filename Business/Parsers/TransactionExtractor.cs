using Business.Extractors;
using Business.Models;

namespace Business.Parsers;

public static class TransactionExtractor
{
    public static List<Transaction> BuildTransactionsFromJson(string json)
    {
        var lines = TextLineExtractor.ExtractTextLinesFromJson(json);
    
        var dates = DateExtractor.ExtractDatesFrom(lines);
        var prices = AmountExtractor.ExtractAmountsFrom(lines);
        var transactionTypes = TransactionTypeExtractor.ExtractTransactionTypesFrom(lines);

        var transactionsTuple = dates.Zip(prices, transactionTypes);

        return transactionsTuple.Select(t =>
                new Transaction(t.First, t.Second, t.Third))
            .ToList(); 
    } 
}