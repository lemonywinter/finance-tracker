using System;
using System.Windows;
using System.Windows.Controls;
using Expenses.Models;
using Expenses.Models.Enums;

namespace FinanceTracker.App
{
    /// <summary>
    /// Interaction logic for AddTransactionDialog.xaml
    /// </summary>
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

            // Initialize and configure category controls based on transaction type
            if (_isIncome)
            {
                CategoryComboBox.ItemsSource = Enum.GetValues<IncomeCategory>();
                CategoryComboBox.SelectedItem = IncomeCategory.Salary;
            }
            else
            {
                CategoryComboBox.ItemsSource = Enum.GetValues<ExpenseCategory>();
                CategoryComboBox.SelectedItem = ExpenseCategory.Life;
            }

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

            var cost = new Cost(amount, (Currency)CurrencyComboBox.SelectedItem!);
            if (_isIncome)
            {
                Transaction = new Income(NameTextBox.Text, cost);
            }
            else
            {
                var category = (ExpenseCategory)CategoryComboBox.SelectedItem!;
                Transaction = new Expense(NameTextBox.Text, cost, category);
            }

            Transaction.Description = DescriptionTextBox.Text;
            Transaction.Recurring = RecurringCheckBox.IsChecked ?? false;

            DialogResult = true;
        }
    }
}