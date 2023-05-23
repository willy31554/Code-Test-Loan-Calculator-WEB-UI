using LoanCalculatorUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Threading.Tasks;

public class LoanController : Controller
{
    private readonly HttpClient _httpClient;

    public LoanController()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("http://localhost:5086/"); // Update the base address with your API endpoint URL
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public ActionResult LoanCalculator()
    {
        var viewModel = new LoanCalculatorViewModel
        {
            Email = "wkipchumba.wk@gmail.com", // Set the email property
            // Other properties
        };

        return View(model: viewModel);
    }

    [HttpPost]
    public async Task<ActionResult> CalculateLoan(LoanCalculatorViewModel model)
    {
        // Retrieve user-entered values from the model
        decimal loanAmount = model.LoanAmount;
        string paymentFrequency = model.PaymentFrequency.ToString();
        int loanPeriod = model.LoanPeriod;
        DateTime startDate = model.StartDate;
        string interestType = model.InterestType.ToString();
        string email = model.Email;
        bool sendByEmail = model.SendByEmail;
        bool downloadAsPdf = model.DownloadAsPdf;

        try
        {
            // Create the loan request model
            var loanRequest = new LoanRequestModel
            {
                LoanAmount = loanAmount,
                PaymentFrequency = paymentFrequency,
                LoanPeriod = loanPeriod,
                StartDate = startDate,
                InterestType = interestType
            };

            // Send the loan request to the API endpoint
            var response = await _httpClient.PostAsJsonAsync("api/loans", loanRequest);
            response.EnsureSuccessStatusCode();

            // Read the response and deserialize it into the loan result model
            var result = await response.Content.ReadAsAsync<LoanResultViewModel>();

            // Prepare data to be displayed in the view
            var viewModel = new LoanResultViewModel
            {
                LoanAmount = result.LoanAmount,
                PaymentFrequency = result.PaymentFrequency,
                LoanPeriod = result.LoanPeriod,
                StartDate = result.StartDate,
                InterestType = result.InterestType,
                InterestRate = result.InterestRate,
                ProcessingFees = result.ProcessingFees,
                ExciseDuty = result.ExciseDuty,
                LegalFees = result.LegalFees,
                Interest = result.Interest,
                TakeHomeAmount = result.TakeHomeAmount,
            };

            // Send email if requested
            if (sendByEmail && !string.IsNullOrEmpty(email))
            {
                var smtpClient = new SmtpClient();
                var mailMessage = new MailMessage("sender@example.com", email)
                {
                    Subject = "Loan Calculation Results",
                    Body = "Loan calculation results:\n\n" +
                           "Loan Amount: " + result.LoanAmount + "\n" +
                           "Payment Frequency: " + result.PaymentFrequency + "\n" +
                           "Loan Period: " + result.LoanPeriod + "\n"
                    // Add more loan result details to the email body
                };

                smtpClient.Send(mailMessage);
            }

            // Generate and download PDF if requested
            if (downloadAsPdf)
            {
                var pdfDocument = new Document();
                var memoryStream = new MemoryStream();
                var writer = PdfWriter.GetInstance(pdfDocument, memoryStream);

                pdfDocument.Open();

                // Add content to the PDF document
                pdfDocument.Add(new Paragraph("Loan Calculation Results"));
                pdfDocument.Add(new Paragraph("Loan Amount: " + result.LoanAmount));
                pdfDocument.Add(new Paragraph("Payment Frequency: " + result.PaymentFrequency));
                pdfDocument.Add(new Paragraph("Loan Period: " + result.LoanPeriod));
                // Add more loan result details to the PDF document

                pdfDocument.Close();

                var pdfBytes = memoryStream.ToArray();

                // Return the PDF file for download
                return File(pdfBytes, "application/pdf", "LoanResult.pdf");
            }

            return View("LoanResult", viewModel);
        }
        catch (Exception)
        {
            // Handle any exceptions or error scenarios
            // You can choose to display an error message or redirect to an error page
            return RedirectToAction("Error", "Home"); // Update with your error handling logic
        }
    }
}
