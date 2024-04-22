using QueueManagementSystem.MVC.Models;
using FastReport;
using FastReport.Export.PdfSimple;
using Microsoft.AspNetCore.Hosting;

namespace QueueManagementSystem.MVC.Services
{
    public class ReportService : IReportService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ReportService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public Report GenerateTicketReport(Ticket ticket)
        {   
            //TODO: catch exceptions for null or invalid tickets
            Report report = new Report();
            string reportPath = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "Ticket.frx");
            report.Load(reportPath);
            report.SetParameterValue("TicketNo", ticket.TicketNumber);
            report.SetParameterValue("serviceID", ticket.ServiceName);
            report.SetParameterValue("printTime", ticket.PrintTime);

            report.Prepare();

            PDFSimpleExport pdfExport = new PDFSimpleExport();
            pdfExport.Export(report, "TicketReport.pdf");

            return report;

        }
    }
}
