using BankAccount.Helpers;
using BankAccount.Interfaces;
using BankAccount.Models;
using FluentAssertions;
using NSubstitute;

namespace BankAccount.Test.Services;

public class StatementServiceTest
{
    private IStatementService _statementService;

    public StatementServiceTest()
    {
        _statementService = Substitute.For<IStatementService>();
    }
    
    [Fact]
    public async Task ShouldReturnFalse_WhenInputFormatIsInvalid()
    {
        // Arrange
        var accounts = new Dictionary<string, Account>();
        var interestRules = new List<InterestRule>();
        var parts = new[] { "1234567890" };

        // Act
        var result = _statementService.ProcessStatement(parts, accounts, interestRules, out int year, out int month);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldReturnFalse_WhenAccountIsInvalid()
    {
        // Arrange
        var accounts = new Dictionary<string, Account>();
        var interestRules = new List<InterestRule>();
        var parts = new[] { "4444444", "202304" };

        // Act
        var result = _statementService.ProcessStatement(parts, accounts, interestRules, out int year, out int month);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldReturnsFalse_WhenYearMonthFormatIsInvalid()
    {
        // Arrange
        var accounts = new Dictionary<string, Account>
                    {
                        { "1234567890", new Account("1234567890") }
                    };
        var interestRules = new List<InterestRule>();
        var parts = new[] { "1234567890", "202313" };

        // Act
        var result = _statementService.ProcessStatement(parts, accounts, interestRules, out int year, out int month);

        // Assert
        result.Should().BeFalse();
    }
}