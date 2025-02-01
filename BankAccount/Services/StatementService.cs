using BankAccount.Helpers;
using BankAccount.Interfaces;
using BankAccount.Models;

namespace BankAccount.Services;

public class StatementService : IStatementService
{
    private readonly BankHelper _bankHelper;

    public StatementService(BankHelper bankHelper)
    {
        _bankHelper = bankHelper;
    }

    public bool Print(Dictionary<string, Account> accounts, List<InterestRule> interestRules)
    {
        Console.WriteLine("\nPlease enter account and month to generate the statement <Account> <Year><Month> \n(or enter blank to go back to main menu):");
        Console.Write("> ");
        string input = Console.ReadLine();

        if (string.IsNullOrEmpty(input))
        {
            return false;
        }

        string[] parts = input.Split(' ');

        if (ProcessStatement(parts, accounts,interestRules, out int year, out int month))
        {
            DisplayAccountStatement(accounts, parts[0], year, month, interestRules);
        }
        else
        {
            Print(accounts, interestRules);
        }

        return true;
    }

    public bool ProcessStatement(string[] parts, Dictionary<string, Account> accounts, List<InterestRule> interestRules,
                                  out int year, out int month)
    {
        month = 0;
        year = 0;
        if (parts.Length != 2)
        {
            Console.WriteLine("Invalid input format. Please try again.");
            return false;
        }

        string account = parts[0];
        if (!accounts.ContainsKey(account))
        {
            Console.WriteLine($"Account {account} not found.");
            return false;
        }

        if (!int.TryParse(parts[1], out int yearMonth) || yearMonth.ToString().Length != 6)
        {
            Console.WriteLine("Invalid year and month format. Please use YYYYMM.");
            return false;
        }

        year = yearMonth / 100;
        month = yearMonth % 100;
        if (month < 1 || month > 12)
        {
            Console.WriteLine("Invalid month. Please enter a valid month (1-12).");
            return false;
        }

        return true;
    }

    public void DisplayAccountStatement(Dictionary<string, Account> accounts, string account, int year, int month, List<InterestRule> interestRules)
    {
        Account acc = accounts[account];
        Console.WriteLine($"\nAccount: {account}");
        Console.WriteLine("| Date     | Txn Id      | Type | Amount | Balance |");

        // Calculate interest for the month
        var totalInterest = CalculateEodBalance(accounts, acc.Number, year, month, interestRules);

        // Display transactions for the month
        double balance = acc.CalculateBalanceForPeriod(year, month);

        // Display transactions for the current month
        bool firstTransaction = true; // Flag to track if it's the first transaction
        int tIDWidth = 0;
        foreach (Transaction t in acc.Transactions.Where(t => int.Parse(t.Date.Substring(0, 6)) == year * 100 + month))
        {
            // Update the balance only after the first transaction
            if (!firstTransaction)
            {
                balance += (t.Type == "D" ? t.Amount : -t.Amount);
            }

            //Get Maximum width of ID for display
            tIDWidth = acc.Transactions.Max(t => t.Id.Length);

            // Display the transaction
            Console.WriteLine($"| {t.Date} | {t.Id.PadRight(tIDWidth)} | {t.Type.PadRight(4)} | {t.Amount:F2} | {balance:F2} |");

            firstTransaction = false; // Set the flag to false after the first transaction
        }

        // Add interest transaction if applicable
        if (totalInterest > 0)
        {
            string interestDate = $"{year:D4}{month:D2}30"; // Assuming interest is credited on the last day of the month
            string interestId = ""; // No transaction ID for interest
            Transaction interestTransaction = new Transaction(interestId, interestDate, "I", totalInterest);
            Console.WriteLine($"| {interestDate} | {interestId.PadRight(tIDWidth)} | {interestTransaction.Type.PadRight(4)} | {interestTransaction.Amount:F2} | {balance + totalInterest:F2} |");
        }
    }

    public double CalculateEodBalance(Dictionary<string, Account> accounts, string account, int year, int month, List<InterestRule> interestRules)
    {
        Account acc = accounts[account];

        // Get the first and last day of the month
        DateTime firstDayOfMonth = new DateTime(year, month, 1);
        DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        var transactions = new List<Transaction>();

        foreach (Transaction t in acc.Transactions)
        {
            transactions.Add(t);
        }

        // Filter transactions for the given year and month
        var monthTransactions = transactions.Where(t =>
                                    DateTime.ParseExact(t.Date, "yyyyMMdd", null).Year == year &&
                                    DateTime.ParseExact(t.Date, "yyyyMMdd", null).Month <= month
                                ).OrderBy(t => DateTime.ParseExact(t.Date, "yyyyMMdd", null)).ToList();

        // Calculate the running balance
        double balance = 0.00;

        // Calculate the end-of-day balance for each period
        List<EodBalance> eodBalances = new List<EodBalance>();
        double totalannualInterest = 0.00;
        double totalInterest = 0.00;
        double currentBalance = balance;
        DateTime currentPeriodStart = firstDayOfMonth;
        string currentRuleId = _bankHelper.GetApplicableRuleId(currentPeriodStart, interestRules);

        bool isFirstTransaction = true;
        int currentNoOfTransaction = 0;

        while (currentPeriodStart <= lastDayOfMonth)
        {
            var filteretedTransactions = new List<Transaction>();
            var currentPeriodEnd = new DateTime();

            // Find the end date of the current period
            if (!isFirstTransaction)
            {
                var currentFirstTransaction = DateTime.ParseExact(monthTransactions.FirstOrDefault().Date, "yyyyMMdd", null);
                currentPeriodEnd = currentPeriodStart.AddDays(_bankHelper.GetDaysForRule(currentRuleId, currentPeriodStart, lastDayOfMonth, interestRules) - 1);
                bool newTransaction = currentFirstTransaction <= currentPeriodEnd;

                if (newTransaction && currentNoOfTransaction != 0)
                    currentPeriodEnd = DateTime.ParseExact(monthTransactions.FirstOrDefault().Date, "yyyyMMdd", null).AddDays(-1);

                filteretedTransactions = monthTransactions.Where(t => DateTime.ParseExact(t.Date, "yyyyMMdd", null) <= currentPeriodEnd &&
                                                                          DateTime.ParseExact(t.Date, "yyyyMMdd", null) >= currentPeriodStart).ToList();

            }
            else
            {
                currentPeriodEnd = currentPeriodStart.AddDays(_bankHelper.GetDaysForRule(currentRuleId, currentPeriodStart, lastDayOfMonth, interestRules) - 1);
                filteretedTransactions = monthTransactions.Where(t => DateTime.ParseExact(t.Date, "yyyyMMdd", null) <= currentPeriodEnd).ToList();
            }

            currentNoOfTransaction = filteretedTransactions.Count();

            foreach (var item in filteretedTransactions)
            {
                monthTransactions.Remove(item); //remove calculated monthtransactions

                if (item.Type == "D")
                {
                    currentBalance += item.Amount;
                }
                else if (item.Type == "W")
                {
                    currentBalance -= item.Amount;
                }
            }
            // Calculate the interest for the period
            double interest = _bankHelper.CalculateInterest(currentBalance, currentRuleId, currentPeriodStart, currentPeriodEnd, interestRules);

            // Add the EOD balance to the list
            eodBalances.Add(new EodBalance
            {
                Period = $"{currentPeriodStart.ToString("yyyyMMdd")} - {currentPeriodEnd.ToString("yyyyMMdd")}",
                NumOfDays = (currentPeriodEnd - currentPeriodStart).Days + 1,
                Balance = currentBalance,
                RateId = currentRuleId,
                Rate = _bankHelper.GetRateForRule(currentRuleId, interestRules),
                AnnualizedInterest = interest
            });

            // Update the balance and rule ID for the next period
            currentPeriodStart = currentPeriodEnd.AddDays(1);
            currentRuleId = _bankHelper.GetApplicableRuleId(currentPeriodStart, interestRules);

            isFirstTransaction = false;
        }

        foreach (var item in eodBalances)
        {
            totalannualInterest += item.AnnualizedInterest;
        }

        totalInterest = totalannualInterest / 365;

        return totalInterest;
    }
}