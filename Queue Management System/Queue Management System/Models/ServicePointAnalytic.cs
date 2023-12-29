namespace Queue_Management_System.Models
{
    public class ServicePointAnalytic{
        public string servicePointID {get; set;}

        public string serviceDescription{get; set;}

        public TimeSpan avgWaitingTime{get; set;}

        public TimeSpan avgServiceTime{get; set;}

        public Int32 totalCustomers{get; set;}

    }
}