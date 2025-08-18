using System.Text.Json;
using System.Text.Json.Serialization;

namespace Expenses.Models;

[JsonDerivedType(typeof(Expense), typeDiscriminator: "expense")]
[JsonDerivedType(typeof(Income), typeDiscriminator: "income")]
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
public abstract class Transaction
{
    [JsonConstructor]
    protected Transaction()
    {
        Name = string.Empty;
        Date = DateTime.Now;
        Amount = new Cost(0);
    }

    protected Transaction(string name, Cost amount)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
            
        Name = name;
        Amount = amount;
        Date = DateTime.Now;
    }

    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Cost Amount { get; set; }
    public string? Category { get; set; }
    public bool Recurring { get; set; }
}