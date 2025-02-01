using BankAccount.Helpers;
using BankAccount.Interfaces;
using BankAccount.Models;
using System.Globalization;

namespace BankAccount.Services;

public class TransactionService : ITransactionService
{
    public int transactionCounter = 1;
    private readonly BankHelper _bankHelper;

    public TransactionService(BankHelper bankHelper)
    {
        _bankHelper = bankHelper;
    }

    public bool Input(Dictionary<string, Account> accounts)
    {
        Console.WriteLine("\nPlease enter transaction details in <Date> <Account> <Type> <Amount> format \n(or enter blank to go back to main menu):");
        Console.Write("> ");
        string input = Console.ReadLine();

        if (string.IsNullOrEmpty(input)) 
        {
            return false;
        }

        if (ProcessTransaction(input, accounts))
        {
            // Display transaction details
            DisplayAccountTransactions(accounts[input.Split(' ')[1]]);
        }
        else
        {
            Input(accounts);
        }

        return true;
    }

    public bool ProcessTransaction(string input, Dictionary<string, Account> accounts)
    {
        string[] parts = input.Split(' ');
        if (parts.Length != 4)
        {
            Console.WriteLine("Invalid transaction format. Please try again.");
            return false;
        }

        if (!_bankHelper.IsValidDate(parts[0]))
        {
            Console.WriteLine("Invalid date format. Please use YYYYMMdd.");
            return false;
        }

        string account = parts[1];
        string type = parts[2].ToUpper();
        if (type != "D" && type != "W")
        {
            Console.WriteLine("Invalid transaction type. Please use D for deposit or W for withdrawal.");
            return false;
        }

        if (!double.TryParse(parts[3], NumberStyles.Number, CultureInfo.InvariantCulture, out double amount) || amount <= 0)
        {
            Console.WriteLine("Invalid amount. Please enter a positive number.");
            return false;
        }

        // Check for exactly 2 decimal places
        if (parts[3].Contains(".") && parts[3].Split('.')[1].Length != 2)
        {
            Console.WriteLine("Invalid amount. Please enter a number with 2 decimal places.");
            return false;
        }

        // Create account if it doesn't exist
        if (!accounts.ContainsKey(account))
        {
            accounts[account] = new Account(account);
        }

        // Check for insufficient funds for withdrawal
        if (type == "W" && accounts[account].Balance < amount)
        {
            Console.WriteLine("Insufficient funds. Withdrawal cannot be processed.");
            return false;
        }

        // Check if the first transaction is a withdrawal
        if (accounts[account].Transactions.Count == 0 && type == "W")
        {
            Console.WriteLine("The first transaction on an account cannot be a withdrawal.");
            return false;
        }

        // Reset transaction counter if the date is different from the last transaction
        if (accounts[account].Transactions.Count > 0 && parts[0] != accounts[account].Transactions.Last().Date)
        {
            transactionCounter = 1;
        }

        // Generate unique transaction ID
        string transactionId = $"{parts[0]}-{transactionCounter:D2}";
        transactionCounter++;

        // Create and add transaction
        Transaction transaction = new Transaction(transactionId, parts[0], type, amount);
        accounts[account].Transactions.Add(transaction);

        return true;
    }

    public void DisplayAccountTransactions(Account account)
    {
        int dateWidth = account.Transactions.Max(t => t.Date.Length);
        int txnIdWidth = account.Transactions.Max(t => t.Id.Length);
        int typeWidth = account.Transactions.Max(t => nameof(t.Type).Length);
        int amountWidth = account.Transactions.Max(t => t.Amount.ToString().Length);

        Console.WriteLine($"\nAccount: {account.Number}");
        Console.WriteLine($"| {"Date".PadRight(dateWidth)} | {"Txn Id".PadRight(txnIdWidth)} | {"Type".PadRight(typeWidth)} | {"Amount".PadRight(amountWidth)} |");
        foreach (Transaction t in account.Transactions)
        {
            Console.WriteLine($"| {t.Date.PadRight(dateWidth)} | {t.Id.PadRight(txnIdWidth)} | {t.Type.PadRight(typeWidth)} | {t.Amount.ToString("F2").PadRight(amountWidth)} |");
        }
    }
}