using System.Linq;
using Expenses.Models;
using Expenses.Models.Enums;
using Expenses.Storage;

namespace Expenses.Services;

public class TransactionManager
{
    private readonly List<Transaction> _transactions;
    private readonly ITransactionStorage _storage;

    public TransactionManager(ITransactionStorage storage)
    {
        _storage = storage;
        _transactions = new List<Transaction>(_storage.LoadTransactions());
    }

    public void AddTransaction(Transaction transaction)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));
            
        _transactions.Add(transaction);
        _storage.SaveTransactions(_transactions);
    }

    public bool DeleteTransaction(Transaction transaction)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));
            
        bool result = _transactions.Remove(transaction);
        if (result)
        {
            _storage.SaveTransactions(_transactions);
        }
        return result;
    }

    public bool DeleteTransactionById(int index)
    {
        if (index < 0 || index >= _transactions.Count)
            return false;
            
        _transactions.RemoveAt(index);
        _storage.SaveTransactions(_transactions);
        return true;
    }

    public IReadOnlyList<Transaction> GetAllTransactions()
    {
        return _transactions.AsReadOnly();
    }

    public IReadOnlyList<Expense> GetExpenses()
    {
        return _transactions.OfType<Expense>().ToList().AsReadOnly();
    }

    public IReadOnlyList<Income> GetIncomes()
    {
        return _transactions.OfType<Income>().ToList().AsReadOnly();
    }

    public Cost GetNetIncome()
    {
        if (!_transactions.Any())
            return new Cost(0);

        Currency currency = _transactions.First().Amount.Currency;
        if (_transactions.Any(t => t.Amount.Currency != currency))
            throw new InvalidOperationException("Cannot calculate net income with mixed currencies");

        Cost totalIncome = _transactions.OfType<Income>()
            .Aggregate(new Cost(0, currency), (sum, income) => sum + income.Amount);

        Cost totalExpenses = _transactions.OfType<Expense>()
            .Aggregate(new Cost(0, currency), (sum, expense) => sum + expense.Amount);

        return totalIncome - totalExpenses;
    }

    public IEnumerable<IGrouping<string?, Transaction>> GetTransactionsByCategory()
    {
        return _transactions.GroupBy(t => t.Category);
    }
}