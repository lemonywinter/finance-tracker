using System.Text.Json;
using System.Text.Json.Serialization;
using Expenses.Models;

namespace Expenses.Storage;

public class JsonFileTransactionStorage : ITransactionStorage
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions;

    public JsonFileTransactionStorage(string filePath)
    {
        _filePath = Path.GetFullPath(filePath);
        Console.WriteLine($"Initializing storage with file: {_filePath}");
        
        _jsonOptions = new JsonSerializerOptions 
        { 
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };
    }

    public IEnumerable<Transaction> LoadTransactions()
    {
        try
        {
            Console.WriteLine($"Attempting to load transactions from: {_filePath}");
            
            if (!File.Exists(_filePath))
            {
                Console.WriteLine("File does not exist, returning empty list");
                return Enumerable.Empty<Transaction>();
            }

            string jsonContent = File.ReadAllText(_filePath);
            Console.WriteLine($"Read content: {jsonContent}");
            
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                Console.WriteLine("File is empty, returning empty list");
                return Enumerable.Empty<Transaction>();
            }

            var result = JsonSerializer.Deserialize<List<Transaction>>(jsonContent, _jsonOptions);
            Console.WriteLine($"Deserialized {result?.Count ?? 0} transactions");
            return result ?? Enumerable.Empty<Transaction>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading transactions: {ex}");
            File.Delete(_filePath);
            return Enumerable.Empty<Transaction>();
        }
    }

    public void SaveTransactions(IEnumerable<Transaction> transactions)
    {
        try
        {
            Console.WriteLine($"Saving transactions to: {_filePath}");
            string jsonContent = JsonSerializer.Serialize(transactions, _jsonOptions);
            Console.WriteLine($"Serialized content: {jsonContent}");
            
            string? directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(_filePath, jsonContent);
            Console.WriteLine("Successfully saved transactions");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving transactions: {ex}");
            throw;
        }
    }
}