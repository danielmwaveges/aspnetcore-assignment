namespace Queue_Management_System.Models
{
    public class Customer
    {
        public string TicketNo {get; set; }
        public string ServicePoint {get; set;}
        public DateTime TimeQueued {get; set;}
        public bool ShowedUp {get; set;} = false;
        public DateTime? TimeShowedUp {get; set;} = null;
        public DateTime? TimeFinished {get; set;} = null;
        
    }
}