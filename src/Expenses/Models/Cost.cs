using System.Text.Json.Serialization;
using Expenses.Models.Enums;

namespace Expenses.Models;

public readonly struct Cost
{
    private readonly decimal _amount;

    [JsonConstructor]
    public Cost(decimal amount, Currency currency)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        _amount = decimal.Round(amount, 2);
        Currency = currency;
    }

    public decimal Amount => _amount;
    public Currency Currency { get; }

    // Overloads for different numeric types
    public Cost(double amount, Currency currency = Currency.USD) 
        : this((decimal)amount, currency) { }

    public Cost(int amount, Currency currency = Currency.USD) 
        : this((decimal)amount, currency) { }

    public Cost(float amount, Currency currency = Currency.USD) 
        : this((decimal)amount, currency) { }

    public override string ToString()
    {
        return $"{_amount:F2} {Currency}";
    }

    public static Cost FromString(string value)
    {
        string[] parts = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2 || !decimal.TryParse(parts[0], out decimal amount))
            throw new ArgumentException("Invalid cost format. Expected format: '10.99 USD'", nameof(value));

        if (!Enum.TryParse<Currency>(parts[1], true, out Currency currency))
            throw new ArgumentException($"Unknown currency: {parts[1]}", nameof(value));

        return new Cost(amount, currency);
    }

    public static Cost operator +(Cost a, Cost b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Cannot add costs with different currencies");
        
        return new Cost(a.Amount + b.Amount, a.Currency);
    }

    public static Cost operator -(Cost a, Cost b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Cannot subtract costs with different currencies");
        
        return new Cost(a.Amount - b.Amount, a.Currency);
    }
}