using System.Collections.Generic;
using Expenses.Models;

namespace Expenses.Storage;

public interface IIncomeStorage
{
    void SaveIncomes(IEnumerable<Income> incomes);
    IEnumerable<Income> LoadIncomes();
}