using BankAccount.Helpers;
using BankAccount.Interfaces;
using BankAccount.Models;

namespace BankAccount.Services;

public class InterestRulesService : IInterestRulesService
{
    private readonly BankHelper _bankHelper;

    public InterestRulesService(BankHelper bankHelper)
    {
        _bankHelper = bankHelper;
    }

    public bool Define(List<InterestRule> interestRules)
    {
        Console.WriteLine("\nPlease enter interest rules details in <Date> <RuleId> <Rate in %> format \n(or enter blank to go back to main menu):");

        Console.Write("> ");
        string input = Console.ReadLine();

        if (string.IsNullOrEmpty(input))
        {
            return false;
        }

        if (ProcessInterestRule(input, interestRules))
        {
            DisplayInterestRules(interestRules);
        }
        else
        {
            Define(interestRules);
        }

        return true;
    }

    public bool ProcessInterestRule(string input, List<InterestRule> interestRules)
    {
        string[] parts = input.Split(' ');
        if (parts.Length != 3)
        {
            Console.WriteLine("Invalid interest rule format. Please try again.");
            return false;
        }

        if (!_bankHelper.IsValidDate(parts[0]))
        {
            Console.WriteLine("Invalid date format. Please use YYYYMMdd.");
            return false;
        }

        string ruleId = parts[1];
        if (!double.TryParse(parts[2], out double rate) || rate <= 0 || rate >= 100)
        {
            Console.WriteLine("Invalid interest rate. Please enter a value between 0 and 100.");
            return false;
        }

        // Remove existing rule for the same date
        interestRules.RemoveAll(r => r.Date == parts[0]);

        // Add new rule
        interestRules.Add(new InterestRule(parts[0], ruleId, rate));

        return true;
    }

    public void DisplayInterestRules(List<InterestRule> interestRules)
    {
        Console.WriteLine("\nInterest rules:");
        Console.WriteLine("| Date     | RuleId | Rate (%) |");
        foreach (InterestRule rule in interestRules.OrderBy(r => r.Date))
        {
            Console.WriteLine($"| {rule.Date} | {rule.RuleId} | {rule.Rate.ToString("F2").PadRight(8)} |");
        }
    }
}