# Bank Account

## Overview

This project is a simple banking system implemented using .NET 8. It supports operations such as inputting banking transactions, calculating interest, and printing account statements. The project includes unit tests using xUnit to ensure correctness.

## Features

- Input banking transactions
- Define interest rules
- Calculate interest based on transaction history
- Print account statements
- Unit tests using xUnit
- Automation Testing using Selenium.WebDriver and LambdaTest (free trial)

## Technologies Used

- **.NET 8** - Core framework for the application
- **C#** - Programming language
- **xUnit** - Unit testing framework
- **Selenium** - Automation Testing

## Getting Started

### Prerequisites

- .NET 8 SDK installed
- Google Chrome version 133 installed
- Profile in https://automation.lambdatest.com/. Get credentials from the LambdaTest Profile by heading to Account Settings > Password & Security tab to be used in automation testing.

### Installation & Setup

1. Clone the repository:
   ```sh
   git clone <repository-url>
   cd BankAccount
   ```
2. Build the project:
   ```sh
   dotnet build
   ```
3. Run the application:
   ```sh
   dotnet run
   ```

## Usage

Upon launching the application, you will be prompted with the following menu:

```
Welcome to AwesomeGIC Bank! What would you like to do?
[T] Input transactions
[I] Define interest rules
[P] Print statement
[Q] Quit
>
```

### Input Transactions

- Enter transaction details in `<Date> <Account> <Type> <Amount>` format.
- Transactions should follow these constraints:
  - Date format: YYYYMMDD
  - Type: D (deposit) or W (withdrawal)
  - Amount: Greater than zero, up to two decimal places
  - Balance should never be negative

Example input:

```
20230626 AC001 W 100.00
```

### Define Interest Rules

- Enter interest rules in `<Date> <RuleId> <Rate in %>` format.
- Constraints:
  - Rate should be between 0 and 100.
  - The latest rule for a given date is kept.

Example input:

```
20230615 RULE03 2.20
```

### Print Statement

- Enter account and month in `<Account> <Year><Month>` format.
- Displays all transactions and interest earned.

Example input:

```
AC001 202306
```

### Quit

- Enter `Q` to exit the application.

## Running Unit Tests

To run the xUnit tests, execute the following command:

1. Go to test directory:
   ```sh
   cd tests/BankAccount.Test
   ```

2. Run the test:
   ```sh
   dotnet test
   ```

The tests ensure correct functionality for:

- Transaction handling
- Interest calculations
- Statement generation

## Running Automated Tests

To run the automated tests, execute the following command:

1. Open the solution on your preffered IDE (e.g. Visual Studio, Visual Studio Code):
2. Navigate to project BankAccount.AutomatedTest.
3. Open `config.json` file.
4. Update the following user and key, found from the LambdaTest Profile by heading to Account Settings > Password & Security tab to be used in automation testing.
:
   ```sh
   user: <username>
   key: <key>
   ```
5. Save Changes.
6. Go to test directory:
   ```sh
   cd tests/BankAccount.AutomatedTest
   ```

7. Build the project:
   ```sh
   dotnet build
   ```

8. Run the test:
   ```sh
   dotnet test
   ```

9. See results in https://automation.lambdatest.com/build?pageType=build.


The tests ensure correct functionality for:

- Interest calculations

## Author

Developed by Missy Macasaet

