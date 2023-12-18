namespace Queue_Management_System.Models
{
    public class Customer
    {
        public string TicketNo {get; set; }
        public string ServicePoint {get; set}
        public string ServiceProvider {get; set}
        public TimeOnly TimeQueued {get; set;}
        public bool ShowedUp {get; set;}
        public TimeOnly? TimeShowedUp {get; set;}
        public TimeOnly? TimeFinished {get; set;}
        
    }
}