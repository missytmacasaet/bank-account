namespace BankAccount.Models;
public class EodBalance
{
    public string Period { get; set; }
    public int NumOfDays { get; set; }
    public double Balance { get; set; }
    public string RateId { get; set; }
    public double Rate { get; set; }
    public double AnnualizedInterest { get; set; }
}
