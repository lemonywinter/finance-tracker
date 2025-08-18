using System;
using System.Windows;
using System.Windows.Threading;

namespace FinanceTracker.App;

public partial class App : Application
{
    public App()
    {
        this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
    }

    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        MessageBox.Show($"An error occurred: {e.Exception.Message}\n\nDetails: {e.Exception}", 
            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        e.Handled = true;
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            MessageBox.Show($"A fatal error occurred: {ex.Message}\n\nDetails: {ex}", 
                "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

