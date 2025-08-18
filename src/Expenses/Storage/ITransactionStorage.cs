using Expenses.Models;

namespace Expenses.Storage;

public interface ITransactionStorage
{
    void SaveTransactions(IEnumerable<Transaction> transactions);
    IEnumerable<Transaction> LoadTransactions();
}