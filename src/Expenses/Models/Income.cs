using Expenses.Models.Enums;

namespace Expenses.Models;

public class Income : Transaction
{
    public IncomeCategory CategoryType { get; set; }
    public override string Category
    {
        get => CategoryType.ToString();
        set => CategoryType = Enum.TryParse<IncomeCategory>(value, out var category) ? category : throw new ArgumentException($"Invalid category: {value}");
    }

    public Income() { }

    public Income(string name, Cost amount) : base(name, amount)
    {
    }
}