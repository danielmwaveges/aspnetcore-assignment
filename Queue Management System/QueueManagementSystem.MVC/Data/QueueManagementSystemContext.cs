using QueueManagementSystem.MVC.Models;
using Microsoft.EntityFrameworkCore;
using QueueManagementSystem.MVC.Data.EntityConfigurations;
using ServiceProvider = QueueManagementSystem.MVC.Models.ServiceProvider;

namespace QueueManagementSystem.MVC.Data
{
    public class QueueManagementSystemContext : DbContext
    {
        public QueueManagementSystemContext(DbContextOptions<QueueManagementSystemContext> options) : base(options)
        {

        }

        public DbSet<Ticket> QueuedTickets {get; set;}
        public DbSet<Service> Services {get; set;}
        public DbSet<ServedTicket> ServedTickets {get; set;}
        public DbSet<ServiceProvider> ServiceProviders {get; set;}
        public DbSet<ServicePoint> ServicePoints {get; set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new TicketEntityTypeConfiguration());
            builder.ApplyConfiguration(new ServiceEntityTypeConfiguration());
            builder.ApplyConfiguration(new ServedTicketEntityTypeConfiguration());
            builder.ApplyConfiguration(new ServiceProviderEntityTypeConfiguration());
            builder.ApplyConfiguration(new ServicePointEntityTypeConfiguration());

        }


    }
}