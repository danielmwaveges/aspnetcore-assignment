using QueueManagementSystem.MVC.Models;
using Microsoft.EntityFrameworkCore;
using QueueManagementSystem.MVC.Data;

namespace QueueManagementSystem.MVC.Services;

public static class TicketQueueStatus
{
    
    public static readonly string InQueue = "In-Queue";
    public static readonly string Called = "Called";
    public static readonly string InService = "In-Service";
    public static readonly string NoShow = "No-Show";

}

public class TicketService : ITicketService
{   
    private readonly QueueManagementSystemContext _queueManagementSystemContext;

    public TicketService(QueueManagementSystemContext qmscontext)
    {
        _queueManagementSystemContext = qmscontext;
    }
    private string GenerateTicketNumber()
    {
        string startTime = "1/06/2010";
        var seconds = (DateTime.Now - DateTime.Parse(startTime)).TotalSeconds;
        return "TN" + Convert.ToString(Convert.ToInt32(seconds % 100000));
    }

    public Ticket GenerateTicket(string serviceName)
    {
        //generate Ticket
        string ticketNumber = GenerateTicketNumber();
        
        var ticket = new Ticket{
            TicketNumber = ticketNumber,
            ServiceName = serviceName,
            PrintTime = DateTime.Now,
            Status = TicketQueueStatus.InQueue
        };
        //add ticket to queue
        AddTicketToQueue(ticket);

        return ticket;
    }

    private void AddTicketToQueue(Ticket ticket)
    {
        _queueManagementSystemContext.QueuedTickets.Add(ticket);
        _queueManagementSystemContext.SaveChanges();

        //event to alert service provider queues of added tickets to their queues
        TicketAddedToQueueEvent?.Invoke();
    }

    public Ticket? GetTicketFromQueue(string serviceName, string calledServicePointName)
    {
        var ticket = _queueManagementSystemContext.QueuedTickets.Where(t => t.ServiceName == serviceName && t.Status == TicketQueueStatus.InQueue)
                                                                .OrderBy(t => t.PrintTime).FirstOrDefault();
        if (ticket!=null)
        {
            ticket.Status = TicketQueueStatus.Called;
            _queueManagementSystemContext.SaveChanges();

            //invoke event to alert waiting page that ticket has been called to service point
            TicketCalledFromQueueEvent?.Invoke((ticket.TicketNumber, calledServicePointName));
        }

        

        return ticket;
    }

    public List<Ticket> GetTicketsByServiceName(string serviceName)
    {
        var tickets = _queueManagementSystemContext.QueuedTickets.Where(t => t.ServiceName == serviceName).OrderBy(t => t.PrintTime).ToList();
        return tickets;
    }
    
    public void UpdateTicketStatus(Ticket ticket, string status)
    {
        var oldStatus = ticket.Status;
        ticket.Status = status;
        _queueManagementSystemContext.SaveChanges();

        if (oldStatus == TicketQueueStatus.Called)
        {
            //invoke remove from called tickets event
            TicketRemovedFromCalledEvent?.Invoke(ticket.TicketNumber);
        }
    }

    public void RemoveTicketFromQueue(Ticket ticket)
    {
        _queueManagementSystemContext.QueuedTickets.Remove(ticket);
        _queueManagementSystemContext.SaveChanges();

        //event to alert service provider queues of added tickets to their queues
        TicketAddedToQueueEvent?.Invoke();
    }

    public void SaveServedTicket(ServedTicket servedTicket)
    {
        _queueManagementSystemContext.ServedTickets.Add(servedTicket);
        _queueManagementSystemContext.SaveChanges();
    }
    
    
    public event Action TicketAddedToQueueEvent; //TODO: rename to include delete and status change operations for this event

    public event Action<(string, string)> TicketCalledFromQueueEvent;

    public event Action<string> TicketRemovedFromCalledEvent;


    //https://docs.google.com/forms/d/e/1FAIpQLSf-i6z0MD7PCosQe50mDYqnoZBLeYC0Gosun-YNGk3XIiNYSg/viewform?pli=1&pli=1
        
}