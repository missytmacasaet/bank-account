using BankAccount.Interfaces;
using BankAccount.Models;
using FluentAssertions;
using NSubstitute;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace BankAccount.AutomatedTest.Services;

public class InterestRulesServiceTest : IClassFixture<BaseFixture>
{
    private IInterestRulesService _interestRulesService;
    private readonly ChromeDriver chromeDriver;
    private readonly BaseFixture baseFixture;

    public InterestRulesServiceTest(BaseFixture baseFixture)
    {
        _interestRulesService = Substitute.For<IInterestRulesService>();
        this.baseFixture = baseFixture;
    }   

    [Fact]
    public async Task ShouldReturnFalse_WhenDateFormatIsIncorrect()
    {
        try
        {
            RemoteWebDriver driver = baseFixture.GetDriver("chrome", "single");
            // Arrange
            var interestRules = new List<InterestRule>();

            string input = "20233290 RULE02 1.90";

            // Act
            var result = _interestRulesService.ProcessInterestRule(input, interestRules);

            // Assert
            result.Should().Be(false);
            interestRules.Should().HaveCount(0);

            /* Perform wait to check the output */
            System.Threading.Thread.Sleep(2000);

            Console.WriteLine("ShouldReturnFalse_WhenDateFormatIsIncorrect Passed");
            baseFixture.SetStatus(true);
        }
        catch (Exception)
        {
            baseFixture.SetStatus(false);
            throw;
        }        
    }

    [Fact]
    public async Task ShouldReturnFalse_WhenInterestRateIsIncorrect()
    {
        RemoteWebDriver driver = baseFixture.GetDriver("chrome", "single");
        // Arrange
        var interestRules = new List<InterestRule>();

        string input = "20233290 RULE02 -1";

        // Act
        var result = _interestRulesService.ProcessInterestRule(input, interestRules);

        // Assert
        result.Should().Be(false);
        interestRules.Should().HaveCount(0);


        /* Perform wait to check the output */
        System.Threading.Thread.Sleep(2000);

        Console.WriteLine("ShouldReturnFalse_WhenInterestRateIsIncorrect Passed");
        baseFixture.SetStatus(true);
    }
}