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

    public Ticket TransferTicket(string ticketNumber, string serviceName)
    {
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
        using var context = _dbFactory.CreateDbContext();
        context.QueuedTickets.Add(ticket);
        context.SaveChanges();

        //event to alert service provider queues of added tickets to their queues
        TicketAddedToQueueEvent?.Invoke(this, EventArgs.Empty);
    }

    public Ticket? GetTicketFromQueue(string serviceName, string calledServicePointName)
    {
        using var context = _dbFactory.CreateDbContext();
        var ticket = context.QueuedTickets.Where(t => t.ServiceName == serviceName && t.Status == TicketQueueStatus.InQueue)
                                                                .OrderBy(t => t.PrintTime).FirstOrDefault();
        if (ticket!=null)
        {
            ticket.Status = TicketQueueStatus.Called;
            context.SaveChanges();

            //invoke event to alert waiting page that ticket has been called to service point
            TicketCalledFromQueueEvent?.Invoke(this, (ticket.TicketNumber, calledServicePointName));

            //event to alert service provider queues of added tickets to their queues
            TicketAddedToQueueEvent?.Invoke(this, EventArgs.Empty);

        }

        return ticket;
    }

    public Ticket? GetNoShowTicket(string serviceName, string calledServicePointName)
    {
        var ticket = NoShowTickets.Where(t => t.ServiceName == serviceName).FirstOrDefault();
        
        if (ticket!=null)
        {
            NoShowTickets.Remove(ticket);
            ticket.Status = TicketQueueStatus.Called;
            Console.WriteLine($"recalled ticket {ticket.Status}");
            using var context = _dbFactory.CreateDbContext();
            context.Update(ticket);
            context.SaveChanges();
        
            //invoke event to alert waiting page that ticket has been called to service point
            TicketCalledFromQueueEvent?.Invoke(this, (ticket.TicketNumber, calledServicePointName));

            //event to alert service provider queues of added tickets to their queues
            TicketAddedToQueueEvent?.Invoke(this, EventArgs.Empty);

        }

        return ticket;
    }

    public List<Ticket> GetTicketsByServiceName(string serviceName)
    {
        using var context = _dbFactory.CreateDbContext();
        var tickets = context.QueuedTickets.Where(t => t.ServiceName == serviceName).OrderBy(t => t.PrintTime).ToList();
        Console.WriteLine(serviceName);
        return tickets;
    }
    
    public void UpdateTicketStatus(int id, string newStatus)
    {
        using var context = _dbFactory.CreateDbContext();
        var ticket = context.QueuedTickets.First(t => t.Id == id);
        
        var oldStatus = ticket.Status;
        ticket.Status = newStatus;

        if (newStatus == TicketQueueStatus.NoShow)
        {
            NoShowTickets.Add(ticket);

        }
        
        context.SaveChanges();

        if (oldStatus == TicketQueueStatus.Called)
        {
            //invoke remove from called tickets event
            TicketRemovedFromCalledEvent?.Invoke(this, ticket.TicketNumber);

            //event to alert service provider queues of tickets change status 
            TicketAddedToQueueEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    public void RemoveTicketFromQueue(Ticket ticket)
    {
        using var context = _dbFactory.CreateDbContext();
        context.QueuedTickets.Remove(ticket);
        context.SaveChanges();

        //event to alert service provider queues of added tickets to their queues
        TicketAddedToQueueEvent?.Invoke(this, EventArgs.Empty);
    }

    public void SaveServedTicket(ServedTicket servedTicket)
    {
        using var context = _dbFactory.CreateDbContext();
        context.ServedTickets.Add(servedTicket);
        context.SaveChanges();
    }
    
    
    public event EventHandler TicketAddedToQueueEvent; //TODO: rename to include delete and status change operations for this event

    public event EventHandler<(string, string)> TicketCalledFromQueueEvent;

    public event EventHandler<string> TicketRemovedFromCalledEvent;


    //https://docs.google.com/forms/d/e/1FAIpQLSf-i6z0MD7PCosQe50mDYqnoZBLeYC0Gosun-YNGk3XIiNYSg/viewform?pli=1&pli=1
        
}