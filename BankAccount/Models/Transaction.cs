namespace BankAccount.Models
{
    public class Transaction
    {
        public string Id { get; }
        public string Date { get; }
        public string Type { get; }
        public double Amount { get; }

        public Transaction(string id, string date, string type, double amount)
        {
            Id = id;
            Date = date;
            Type = type;
            Amount = amount;
        }
    }
}
