namespace BankAccount.Models
{
    public class Account
    {
        public string Number { get; }
        public List<Transaction> Transactions { get; } = new List<Transaction>();

        public Account(string number)
        {
            Number = number;
        }

        // Method to calculate balance for a specific period
        public double CalculateBalanceForPeriod(int year, int month)
        {
            // Get transactions from the previous month
            List<Transaction> previousMonthTransactions = Transactions.Where(t => int.Parse(t.Date.Substring(0, 6)) == (year * 100 + month - 1)).ToList();

            // Get the first transaction of the current month
            Transaction firstTransaction = Transactions.FirstOrDefault(t => int.Parse(t.Date.Substring(0, 6)) == year * 100 + month);

            // Calculate the balance
            double balance = previousMonthTransactions.Sum(t => t.Type == "D" ? t.Amount : -t.Amount);
            if (firstTransaction != null)
            {
                balance += (firstTransaction.Type == "D" ? firstTransaction.Amount : -firstTransaction.Amount);
            }

            return balance;
        }

        public double Balance
        {
            get
            {
                return Transactions.Sum(t => t.Type == "D" ? t.Amount : -t.Amount);
            }
        }

    }
}
