using BankAccount.Models;

namespace BankAccount.Interfaces;

public interface ITransactionService
{
    bool Input(Dictionary<string, Account> accounts);
    bool ProcessTransaction(string input, Dictionary<string, Account> accounts);
}