using System.Collections.Generic;
using Expenses.Models;

namespace Expenses.Storage;

public interface IExpenseStorage
{
    void SaveExpenses(IEnumerable<Expense> expenses);
    IEnumerable<Expense> LoadExpenses();
}