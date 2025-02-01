using BankAccount.Helpers;
using BankAccount.Interfaces;
using BankAccount.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BankAccount;

class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Welcome to AwesomeGIC Bank! What would you like to do?");
        var services = CreateServices();
        var app = services.GetRequiredService<IBankService>();
        app.HandleChoice();
    }

    private static ServiceProvider CreateServices()
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging(options =>
            {
                options.ClearProviders();
                options.AddConsole();
            })
            .AddSingleton<IBankService, BankService>()
            .AddSingleton<IInterestRulesService, InterestRulesService>()
            .AddSingleton<ITransactionService, TransactionService>()
            .AddSingleton<IStatementService, StatementService>()
            .AddSingleton<BankHelper>()
            .BuildServiceProvider();
        return serviceProvider;
    }
}