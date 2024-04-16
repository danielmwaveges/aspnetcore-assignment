using System;
using QueueManagementSystem.MVC.Models;

namespace QueueManagementSystem.MVC.Services
{
    public interface ITicketService
    {
        Ticket GenerateTicket(string serviceName);

        Ticket? GetTicketFromQueue(string serviceName, string calledServicePointName);

        List<Ticket> GetTicketsByServiceName(string serviceName);

        void UpdateTicketStatus(Ticket ticket, string status);

        void RemoveTicketFromQueue(Ticket ticket);

        void SaveServedTicket(ServedTicket servedTicket);


        // Declare the event.
        public event Action TicketAddedToQueueEvent;

        public event Action<(string, string)> TicketCalledFromQueueEvent;

        public event Action<string> TicketRemovedFromCalledEvent;
    }
}
