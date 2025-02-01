# Bank Account

## Overview

This project is a simple banking system implemented using .NET 8. It supports operations such as inputting banking transactions, calculating interest, and printing account statements. The project includes unit tests using xUnit to ensure correctness.

## Features

- Input banking transactions
- Define interest rules
- Calculate interest based on transaction history
- Print account statements
- Unit tests using xUnit

## Technologies Used

- **.NET 8** - Core framework for the application
- **C#** - Programming language
- **xUnit** - Unit testing framework

## Getting Started

### Prerequisites

- .NET 8 SDK installed

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

## Author

Developed by Missy Macasaet

