using BankAccount.Interfaces;
using BankAccount.Models;

namespace BankAccount.Helpers;

public class BankHelper
{

    public string GetApplicableRuleId(DateTime date, List<InterestRule> interestRules)
    {
        var rules = interestRules.Where(r => DateTime.ParseExact(r.Date, "yyyyMMdd", null) <= date).OrderByDescending(r => DateTime.ParseExact(r.Date, "yyyyMMdd", null)).FirstOrDefault();

        return rules.RuleId;
    }

    public int GetDaysForRule(string ruleId, DateTime periodStart, DateTime periodEnd, List<InterestRule> interestRules)
    {
        var nextRuleChangeDate = DateTime.Now;

        var dateStart = interestRules.Where(r => r.RuleId != ruleId && DateTime.ParseExact(r.Date, "yyyyMMdd", null) > periodStart).OrderBy(r => DateTime.ParseExact(r.Date, "yyyyMMdd", null)).FirstOrDefault()?.Date;
        if (dateStart == null)
        {
            nextRuleChangeDate = periodEnd.AddDays(1);
        }
        else
        {
            nextRuleChangeDate = DateTime.ParseExact(dateStart, "yyyyMMdd", null);
        }

        // Calculate the number of days for the current rule
        return (nextRuleChangeDate - periodStart).Days;
    }

    public double GetRateForRule(string ruleId, List<InterestRule> interestRules)
    {
        return interestRules.FirstOrDefault(r => r.RuleId == ruleId)?.Rate ?? 0;
    }

    public double CalculateInterest(double balance, string ruleId, DateTime periodStart, DateTime periodEnd, List<InterestRule> interestRules)
    {
        double rate = GetRateForRule(ruleId, interestRules);
        int days = (periodEnd - periodStart).Days + 1;
        return balance * (rate / 100) * days;
    }

    public string GetChoices()
    {
        Console.WriteLine("[T] Input transactions");
        Console.WriteLine("[I] Define interest rules");
        Console.WriteLine("[P] Print statement");
        Console.WriteLine("[Q] Quit");
        Console.Write("> ");
        return Console.ReadLine().ToUpper();
    }

    public bool IsValidDate(string dateString)
    {
        if (dateString.Length != 8)
        {
            return false;
        }

        if (!int.TryParse(dateString, out int date))
        {
            return false;
        }

        try
        {
            DateTime.ParseExact(dateString, "yyyyMMdd", null);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}