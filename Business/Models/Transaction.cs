using System;

namespace Business.Models
{
    public record Transaction(DateTime Date, decimal Amount, string TransactionType);
}