using BankAccount.Interfaces;
using BankAccount.Models;
using FluentAssertions;
using NSubstitute;

namespace BankAccount.Test.Services;

public class TransactionServiceTest
{
    public class TransactionServiceTests
    {
        private readonly ITransactionService _transactionService;

        public TransactionServiceTests()
        {
            _transactionService = Substitute.For<ITransactionService>();
        }        

        [Theory]
        [InlineData("20231225 12345 D 100")] // Missing decimal places
        [InlineData("20231225 12345 D 100.0")] // Missing decimal places
        [InlineData("20231225 12345 D 100.000")] // Too many decimal places
        [InlineData("20231225 12345 D -100.00")] // Negative amount
        [InlineData("20231225 12345 D 0.00")] // Zero amount
        public void ShouldReturnsFalse_WhenAmountIsInvalid(string input)
        {
            // Arrange
            var accounts = new Dictionary<string, Account>
            {
                { "12345", new Account("12345") }
            };

            // Act
            var result = _transactionService.ProcessTransaction(input, accounts);

            // Assert
            result.Should().BeFalse();
        }
    }
}