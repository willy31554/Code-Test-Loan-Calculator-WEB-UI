namespace LoanCalculatorUI.Models
    {
        public class LoanResultViewModel
        {
            public decimal LoanAmount { get; set; }
            public required string PaymentFrequency { get; set; }
            public int LoanPeriod { get; set; }
            public DateTime StartDate { get; set; }
            public required string InterestType { get; set; }
            public decimal InterestRate { get; set; }
            public decimal ProcessingFees { get; set; }
            public decimal ExciseDuty { get; set; }
            public decimal LegalFees { get; set; }
            public decimal Interest { get; set; }
            public decimal TakeHomeAmount { get; set; }
        }
    }

