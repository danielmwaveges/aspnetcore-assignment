

namespace QueueManagementSystem.MVC.Models
{
    public class Ticket
    {
        public int Id{get; set;}
        public string TicketNumber{get; set;}
        public string ServiceName {get; set;}
        public DateTime PrintTime {get; set;}
        public string Status {get; set;}
    }
}