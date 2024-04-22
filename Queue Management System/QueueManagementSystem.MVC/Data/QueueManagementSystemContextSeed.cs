using QueueManagementSystem.MVC.Models;
using Microsoft.EntityFrameworkCore;


using ServiceProvider = QueueManagementSystem.MVC.Models.ServiceProvider;

namespace QueueManagementSystem.MVC.Data
{
    public class QueueManagementSystemContextSeeder
    {
        public static void SeedServices(QueueManagementSystemContext context)
        {
            if (!context.Services.Any())
            {
                var services = new List<Service>
                {
                    new Service {Id = -1, Name = "Dental Service", Description="Comprehensive dental care, from routine cleanings and exams to advanced procedures like implants and oral surgery."},
                    new Service {Id = -2, Name = "Diagnostic Service", Description="Comprehensive range of tests, from imaging (X-ray, CT, MRI) to lab work,  to pinpoint the cause of your health concerns."},
                    new Service {Id = -3, Name = "Surgical Service", Description="Advanced techniques to perform a wide range of procedures, from minimally invasive to complex surgeries."},
                    new Service {Id = -4, Name = "Rehabilitation Service", Description="Regain strength, mobility, and independence with our hospital's rehabilitation services."},
                    new Service {Id = -5, Name = "Preventive Care Service", Description="Screenings, plans & guidance to help you prevent chronic diseases, catch issues early & live healthier."},

                };

                context.AddRange(services);
                context.SaveChanges();

            }
        }

        public static void SeedServicePoints(QueueManagementSystemContext context)
        {
            if (!context.ServicePoints.Any())
            {
                var servicePoints = new List<ServicePoint>
                {
                    new ServicePoint {Id = -1, Name = "ServicePoint-1", ServiceId = -1},
                    new ServicePoint {Id = -2, Name = "ServicePoint-2", ServiceId = -2},
                    new ServicePoint {Id = -3, Name = "ServicePoint-3", ServiceId = -2},
                    new ServicePoint {Id = -4, Name = "ServicePoint-4", ServiceId = -3},
                    new ServicePoint {Id = -5, Name = "ServicePoint-5", ServiceId = -4},
                    new ServicePoint {Id = -6, Name = "ServicePoint-6", ServiceId = -5},

                };

                context.AddRange(servicePoints);
                context.SaveChanges();

            }
        }

        public static void SeedServiceProviders(QueueManagementSystemContext context)
        {
            if (!context.ServiceProviders.Any())
            {
                var serviceProviders = new List<ServiceProvider>
                {
                    new ServiceProvider {Id = -1, FullNames = "Daniel Mwambogha", Email = "dante@gmail.com", IsAdmin = true, Password = BCrypt.Net.BCrypt.HashPassword("Dante@09"), ServicePointId = -4},
                    new ServiceProvider {Id = -2, FullNames = "Chris Omar", Email = "chris@gmail.com", IsAdmin = false, Password = BCrypt.Net.BCrypt.HashPassword("Chris@09"), ServicePointId = -2},
                    new ServiceProvider {Id = -3, FullNames = "Amina Mohammed", Email = "amina@yahoo.com", IsAdmin = false, Password = BCrypt.Net.BCrypt.HashPassword("Amina@09"), ServicePointId = -3},
                };

                context.AddRange(serviceProviders);
                context.SaveChanges();

            }
        }
    }
}
