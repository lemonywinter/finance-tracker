using System.Windows;
using Expenses.Models;
using Expenses.Models.Enums;

namespace FinanceTracker.UI;

public partial class AddTransactionDialog : Window
{
    private readonly bool _isIncome;
    public Transaction? Transaction { get; private set; }

    public AddTransactionDialog(bool isIncome)
    {
        InitializeComponent();
        _isIncome = isIncome;
        Title = $"Add {(_isIncome ? "Income" : "Expense")}";

        // Initialize currency combo box
        CurrencyComboBox.ItemsSource = Enum.GetValues<Currency>();
        CurrencyComboBox.SelectedItem = Currency.USD;

        SaveButton.Click += SaveButton_Click;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameTextBox.Text))
        {
            MessageBox.Show("Please enter a name.", "Validation Error",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!decimal.TryParse(AmountTextBox.Text, out decimal amount) || amount <= 0)
        {
            MessageBox.Show("Please enter a valid positive amount.", "Validation Error",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var cost = new Cost(amount, (Currency)CurrencyComboBox.SelectedItem);
        Transaction = _isIncome
            ? new Income(NameTextBox.Text, cost)
            : new Expense(NameTextBox.Text, cost);

        Transaction.Description = DescriptionTextBox.Text;
        Transaction.Category = CategoryTextBox.Text;
        Transaction.Recurring = RecurringCheckBox.IsChecked ?? false;

        DialogResult = true;
    }
}