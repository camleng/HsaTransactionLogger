using Business.Extractors;
using Business.Parsers;

const string fileName = "hsa.json";
var json = File.ReadAllText(fileName);

var transactions = TransactionExtractor.BuildTransactionsFromJson(json);

foreach (var transaction in transactions)
{
    Console.WriteLine(transaction);
}
