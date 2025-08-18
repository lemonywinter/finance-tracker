using System.Linq;
using System.Windows;
using Expenses.Models;
using Expenses.Services;
using Expenses.Storage;

namespace FinanceTracker.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly TransactionManager _transactionManager;

    public MainWindow()
    {
        InitializeComponent();

        // Initialize transaction manager with JSON storage
        var storage = new JsonFileTransactionStorage("transactions.json");
        _transactionManager = new TransactionManager(storage);

        // Load initial data
        RefreshTransactions();

        // Wire up event handlers
        AddIncomeButton.Click += AddIncomeButton_Click;
        AddExpenseButton.Click += AddExpenseButton_Click;
        RemoveButton.Click += RemoveButton_Click;
    }

    private void RefreshTransactions()
    {
        TransactionsGrid.ItemsSource = _transactionManager.GetAllTransactions();
        UpdateSummary();
    }

    private void UpdateSummary()
    {
        var incomes = _transactionManager.GetIncomes();
        var expenses = _transactionManager.GetExpenses();

        var totalIncome = incomes.Sum(i => i.Amount.Amount);
        var totalExpenses = expenses.Sum(e => e.Amount.Amount);
        var netIncome = totalIncome - totalExpenses;

        TotalIncomeText.Text = $"${totalIncome:F2}";
        TotalExpensesText.Text = $"${totalExpenses:F2}";
        NetIncomeText.Text = $"${netIncome:F2}";
    }

    private void AddIncomeButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new AddTransactionDialog(isIncome: true) { Owner = this };
        if (dialog.ShowDialog() == true && dialog.Transaction != null)
        {
            _transactionManager.AddTransaction(dialog.Transaction);
            RefreshTransactions();
        }
    }

    private void AddExpenseButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new AddTransactionDialog(isIncome: false) { Owner = this };
        if (dialog.ShowDialog() == true && dialog.Transaction != null)
        {
            _transactionManager.AddTransaction(dialog.Transaction);
            RefreshTransactions();
        }
    }

    private void RemoveButton_Click(object sender, RoutedEventArgs e)
    {
        var selectedTransaction = TransactionsGrid.SelectedItem as Transaction;
        if (selectedTransaction == null)
        {
            MessageBox.Show("Please select a transaction to remove.", "No Selection", 
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        _transactionManager.DeleteTransaction(selectedTransaction);
        RefreshTransactions();
    }
}