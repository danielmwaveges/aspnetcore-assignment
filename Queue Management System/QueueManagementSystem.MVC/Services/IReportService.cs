using QueueManagementSystem.MVC.Models;
using FastReport;

namespace QueueManagementSystem.MVC.Services
{
    public interface IReportService
    {
        Report GenerateTicketReport(Ticket ticket);

    }
}