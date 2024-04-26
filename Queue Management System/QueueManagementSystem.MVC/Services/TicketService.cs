using QueueManagementSystem.MVC.Models;
using Microsoft.EntityFrameworkCore;
using QueueManagementSystem.MVC.Data;

namespace QueueManagementSystem.MVC.Services;

public static class TicketStatus
{
    public static readonly string InQueue = "In-Queue";
    public static readonly string Called = "Called";
    public static readonly string InService = "In-Service";
    public static readonly string NoShow = "No-Show";
}

public class TicketService : ITicketService
{   
    private readonly IDbContextFactory<QueueManagementSystemContext> _dbFactory;

    private static readonly List<Ticket> NoShowTickets = new List<Ticket> ();

    public TicketService(IDbContextFactory<QueueManagementSystemContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    private string GenerateTicketNumber()
    {
        string startTime = "1/06/2010";
        var seconds = (DateTime.Now - DateTime.Parse(startTime)).TotalSeconds;
        return "TN" + Convert.ToString(Convert.ToInt32(seconds % 100000));
    }

    public Ticket GenerateTicket(string serviceName)
    {
        string ticketNumber = GenerateTicketNumber();
        
        var ticket = new Ticket{
            TicketNumber = ticketNumber,
            ServiceName = serviceName,
            PrintTime = DateTime.Now,
            Status = TicketStatus.InQueue
        };

        AddTicketToQueue(ticket);

        return ticket;
    }

    public Ticket TransferTicket(string ticketNumber, string serviceName)
    {
        var ticket = new Ticket{
            TicketNumber = ticketNumber,
            ServiceName = serviceName,
            PrintTime = DateTime.Now,
            Status = TicketStatus.InQueue
        };
    
        AddTicketToQueue(ticket);

        return ticket;
    }
    private void AddTicketToQueue(Ticket ticket)
    {
        using var context = _dbFactory.CreateDbContext();
        context.QueuedTickets.Add(ticket);
        context.SaveChanges();

        TicketQueueAlteredEvent?.Invoke(this, EventArgs.Empty);
    }

    public Ticket? GetTicketFromQueue(string serviceName, string calledServicePointName)
    {
        using var context = _dbFactory.CreateDbContext();
        var ticket = context.QueuedTickets.Where(t => t.ServiceName == serviceName && t.Status == TicketStatus.InQueue)
                                                                .OrderBy(t => t.PrintTime).FirstOrDefault();
        if (ticket!=null)
        {
            ticket.Status = TicketStatus.Called;
            context.SaveChanges();

            TicketCalledFromQueueEvent?.Invoke(this, (ticket.TicketNumber, calledServicePointName));

            TicketQueueAlteredEvent?.Invoke(this, EventArgs.Empty);
        }

        return ticket;
    }

    public Ticket? GetNoShowTicket(string serviceName, string calledServicePointName)
    {
        var ticket = NoShowTickets.Where(t => t.ServiceName == serviceName).FirstOrDefault();
        
        if (ticket!=null)
        {
            NoShowTickets.Remove(ticket);
            ticket.Status = TicketStatus.Called;
            using var context = _dbFactory.CreateDbContext();
            context.Update(ticket);
            context.SaveChanges();

            TicketCalledFromQueueEvent?.Invoke(this, (ticket.TicketNumber, calledServicePointName));

            TicketQueueAlteredEvent?.Invoke(this, EventArgs.Empty);
        }

        return ticket;
    }

    public List<Ticket> GetTicketsByServiceName(string serviceName)
    {
        using var context = _dbFactory.CreateDbContext();
        var tickets = context.QueuedTickets.Where(t => t.ServiceName == serviceName).OrderBy(t => t.PrintTime).ToList();
        return tickets;
    }
    
    public void UpdateTicketStatus(int id, string newStatus)
    {
        using var context = _dbFactory.CreateDbContext();
        var ticket = context.QueuedTickets.First(t => t.Id == id);
        
        var oldStatus = ticket.Status;
        ticket.Status = newStatus;

        if (newStatus == TicketStatus.NoShow)
        {
            NoShowTickets.Add(ticket);

        }
        
        context.SaveChanges();

        if (oldStatus == TicketStatus.Called)
        {
            TicketRemovedFromCalledEvent?.Invoke(this, ticket.TicketNumber);
        }
 
        TicketQueueAlteredEvent?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveTicketFromQueue(Ticket ticket)
    {
        using var context = _dbFactory.CreateDbContext();
        context.QueuedTickets.Remove(ticket);
        context.SaveChanges();

        TicketQueueAlteredEvent?.Invoke(this, EventArgs.Empty);
    }

    public void SaveServedTicket(ServedTicket servedTicket)
    {
        using var context = _dbFactory.CreateDbContext();
        context.ServedTickets.Add(servedTicket);
        context.SaveChanges();
    }
    
    public event EventHandler TicketQueueAlteredEvent;

    public event EventHandler<(string, string)> TicketCalledFromQueueEvent;

    public event EventHandler<string> TicketRemovedFromCalledEvent;
      
}