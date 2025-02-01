using BankAccount.Models;

namespace BankAccount.Interfaces;

public interface IInterestRulesService
{
    bool Define(List<InterestRule> interestRules);
    bool ProcessInterestRule(string input, List<InterestRule> interestRules);
    void DisplayInterestRules(List<InterestRule> interestRules);
}