namespace Expenses.Models;

public class Expense : Transaction
{
    public Expense() { }

    public Expense(string name, Cost amount) : base(name, amount)
    {
    }
}