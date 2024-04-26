using QueueManagementSystem.MVC.Models;
using FastReport;

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

            return report;
        }

        public Report GenerateAnalyticalReport(List<ServiceStat> serviceStats)
        {
            Report report = new Report();
            string reportPath = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "ServiceStats.frx");
            report.Load(reportPath);
            report.Dictionary.RegisterData(serviceStats, "serviceStats", true);
            DataBand db1 = (DataBand)report.FindObject("Data1");
            db1.DataSource = report.GetDataSource("serviceStats");
            report.Prepare();

            return report;
        }
    }
}
