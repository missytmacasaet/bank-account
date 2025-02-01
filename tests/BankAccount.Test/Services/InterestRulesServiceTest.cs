using BankAccount.Helpers;
using BankAccount.Interfaces;
using BankAccount.Models;
using FluentAssertions;
using NSubstitute;

namespace BankAccount.Test.Services;

public class InterestRulesServiceTest
{
    private IInterestRulesService _interestRulesService;

    public InterestRulesServiceTest()
    {
        _interestRulesService = Substitute.For<IInterestRulesService>();
    }   

    [Fact]
    public async Task ShouldReturnFalse_WhenDateFormatIsIncorrect()
    {
        // Arrange
        var interestRules = new List<InterestRule>();

        string input = "20233290 RULE02 1.90";

        // Act
        var result =_interestRulesService.ProcessInterestRule(input, interestRules);

        // Assert
        result.Should().Be(false);
        interestRules.Should().HaveCount(0);
    }

    [Fact]
    public async Task ShouldReturnFalse_WhenInterestRateIsIncorrect()
    {
        // Arrange
        var interestRules = new List<InterestRule>();

        string input = "20233290 RULE02 -1";

        // Act
        var result = _interestRulesService.ProcessInterestRule(input, interestRules);

        // Assert
        result.Should().Be(false);
        interestRules.Should().HaveCount(0);
    }
}