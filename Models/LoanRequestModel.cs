namespace LoanCalculatorUI.Models
{
    public class LoanRequestModel
    {
        public decimal LoanAmount { get; set; }
        public required string PaymentFrequency { get; set; }
        public int LoanPeriod { get; set; }
        public DateTime StartDate { get; set; }
        public required string InterestType { get; set; }
    }
}