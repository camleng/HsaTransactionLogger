using System;
using System.IO;
using Business.Extractors;

const string fileName = "hsa.json";
var json = File.ReadAllText(fileName);

var transactions = TransactionExtractor.BuildTransactionsFromJson(json);

foreach (var transaction in transactions)
{
    Console.WriteLine(transaction);
}
