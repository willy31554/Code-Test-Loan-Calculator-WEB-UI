namespace LoanCalculatorUI.Models { 
public class LoanCalculatorViewModel
{
    public decimal LoanAmount { get; set; }
    public PaymentFrequency PaymentFrequency { get; set; }
    public int LoanPeriod { get; set; }
    public DateTime StartDate { get; set; }
    public InterestType InterestType { get; set; }
        public required string Email { get; set; }
        public bool SendByEmail { get; set; }
        public bool DownloadAsPdf { get; set; }

    }

    public enum PaymentFrequency
{
    Annually,
    Quarterly,
    Monthly,
    EverySixMonths
}

public enum InterestType
{
    FlatRate,
    ReducingBalance
}
}