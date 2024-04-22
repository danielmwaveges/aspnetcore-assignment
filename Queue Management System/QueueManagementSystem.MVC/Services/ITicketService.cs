using System;
using System.Speech.Synthesis;
using QueueManagementSystem.MVC.Models;

namespace QueueManagementSystem.MVC.Services
{
    public interface ITicketService
    {
        Ticket GenerateTicket(string serviceName);

        Ticket TransferTicket(string ticketNumber, string serviceName);

        Ticket? GetTicketFromQueue(string serviceName, string calledServicePointName);

        Ticket? GetNoShowTicket(string serviceName, string calledServicePointName);

        List<Ticket> GetTicketsByServiceName(string serviceName);

        void UpdateTicketStatus(int id, string status);

        void RemoveTicketFromQueue(Ticket ticket);

        void SaveServedTicket(ServedTicket servedTicket);


        // Declare the event.
        public event EventHandler TicketAddedToQueueEvent;

        public event EventHandler<(string, string)> TicketCalledFromQueueEvent;

        public event EventHandler<string> TicketRemovedFromCalledEvent;
    }
}
