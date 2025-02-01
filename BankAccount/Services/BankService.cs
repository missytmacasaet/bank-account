using BankAccount.Helpers;
using BankAccount.Interfaces;
using BankAccount.Models;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace BankAccount.Services;

public class BankService :IBankService
{
    private readonly ILogger<BankService> _logger;
    public Dictionary<string, Account> accounts = new Dictionary<string, Account>();
    public List<InterestRule> interestRules = new List<InterestRule>();
    private readonly BankHelper _bankHelper;
    private readonly ITransactionService _transactionService;
    private readonly IInterestRulesService _interestRulesService;
    private readonly IStatementService _statementService;

    public BankService(ILogger<BankService> logger, BankHelper bankHelper, ITransactionService transactionService, 
                       IInterestRulesService interestRulesService, IStatementService statementService)
    {
        _logger = logger;
        _bankHelper = bankHelper;
        _transactionService = transactionService;
        _interestRulesService = interestRulesService;
        _statementService = statementService;
    }

    // HandleChoice method
    public void HandleChoice()
    {
        string choice = _bankHelper.GetChoices();

        switch (choice)
        {
            case "T":
                if (!_transactionService.Input(accounts)) 
                {
                    HandleChoice();
                }
                else
                {
                    MoreTransaction();
                };
                break;
            case "I":
                if (!_interestRulesService.Define(interestRules))
                {
                    HandleChoice();
                }
                else
                {
                    MoreTransaction();
                };
                break;
            case "P":
                if (!_statementService.Print(accounts, interestRules)) HandleChoice();
                break;
            case "Q":
                Console.WriteLine("Thank you for banking with AwesomeGIC Bank.\nHave a nice day!");
                break;
            default:
                Console.WriteLine("Incorrect input. Please try again.");
                HandleChoice();
                break;
        }
    }

    public void MoreTransaction()
    {
        // Display choices again
        Console.WriteLine("\nIs there anything else you'd like to do?");
        HandleChoice();
    }
}