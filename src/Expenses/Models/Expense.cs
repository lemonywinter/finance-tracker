using Expenses.Models.Enums;

namespace Expenses.Models;

public class Expense : Transaction
{
    public ExpenseCategory CategoryType { get; set; }
    public override string Category
    {
        get => CategoryType.ToString();
        set => CategoryType = Enum.TryParse<ExpenseCategory>(value, out var category) ? category : throw new ArgumentException($"Invalid category: {value}");
    }

    public Expense() { }

    public Expense(string name, Cost amount, ExpenseCategory category = ExpenseCategory.Life) : base(name, amount)
    {
        CategoryType = category;
    }
}