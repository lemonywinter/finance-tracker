namespace Expenses.Models;

public class Income : Transaction
{
    public Income() { }

    public Income(string name, Cost amount) : base(name, amount)
    {
    }
}