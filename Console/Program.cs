using System;
using System.IO;
using System.Text.Json;
using Business.Extractors;
using Business.Models;

const string fileName = "hsa.json";
var json = File.ReadAllText(fileName);

var hsaResult = JsonSerializer.Deserialize<HsaResult>(json);

var transactions = TransactionBuilder.BuildTransactions(hsaResult);

foreach (var transaction in transactions)
{
    Console.WriteLine(transaction);
}
