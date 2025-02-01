namespace BankAccount.Models;
public class InterestRule
{
    public string Date { get; }
    public string RuleId { get; }
    public double Rate { get; }

    public InterestRule(string date, string ruleId, double rate)
    {
        Date = date;
        RuleId = ruleId;
        Rate = rate;
    }
}