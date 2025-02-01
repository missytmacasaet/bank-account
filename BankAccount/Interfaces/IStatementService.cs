using BankAccount.Models;

namespace BankAccount.Interfaces;

public interface IStatementService
{
    bool Print(Dictionary<string, Account> accounts, List<InterestRule> interestRules);
    bool ProcessStatement(string[] parts, Dictionary<string, Account> accounts, List<InterestRule> interestRules,
                                  out int year, out int month);
    void DisplayAccountStatement(Dictionary<string, Account> accounts, string account, int year, int month, List<InterestRule> interestRules);

    double CalculateEodBalance(Dictionary<string, Account> accounts, string account, int year, int month, List<InterestRule> interestRules);
}