using Queue_Management_System.Models;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace Queue_Management_System.Services
{
    public class CustomerDbService
    {
        private readonly string _connectionString;

        public CustomerDbService(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async void addCustomerToDb(Customer customer)
        {
            var connectionString = _connectionString;
            await using var dataSource = NpgsqlDataSource.Create(connectionString);

            string tktno = customer.TicketNo;
            string servicepointid = customer.ServicePoint;
            DateTime queuetime = customer.TimeQueued;
            bool showed = customer.ShowedUp;
            DateTime? showtime = customer.TimeShowedUp;
            if (showtime == null)
            {
                showtime = DateTime.MaxValue;
            }
            DateTime? finishtime = customer.TimeFinished;
            if (finishtime == null)
            {
                finishtime = DateTime.MaxValue;
            }
            

            string querystring = $"INSERT INTO servedcustomers (\"TicketNumber\", \"ServicePointID\", \"TimeQueued\", \"ShowedUp\", \"TimeShowedUp\", \"TimeFinished\") VALUES ('{tktno}', '{servicepointid}', '{queuetime}', '{showed}', '{showtime}', '{finishtime}')";
            Console.WriteLine(querystring);
            await using var command = dataSource.CreateCommand(querystring);
            await command.ExecuteNonQueryAsync();
        }

        public async void updateCustomer(Customer customer)
        {
            var connectionString = _connectionString;
            await using var dataSource = NpgsqlDataSource.Create(connectionString);

            string tktno = customer.TicketNo;
            bool showed = customer.ShowedUp;
            DateTime? showtime = customer.TimeShowedUp;
            if (showtime == null)
            {
                showtime = DateTime.MaxValue;
            }
            DateTime? finishtime = customer.TimeFinished;
            if (finishtime == null)
            {
                finishtime = DateTime.MaxValue;
            }
            string querystring = $"UPDATE servedcustomers SET (\"ShowedUp\", \"TimeShowedUp\", \"TimeFinished\") = ('{showed}', '{showtime}', '{finishtime}') WHERE \"TicketNumber\" = '{tktno}'";
            Console.WriteLine(querystring);
            await using var command = dataSource.CreateCommand(querystring);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<ServicePointAnalytic[]> getServicePointsAnalytics()
        {
            var connectionString = _connectionString;
            await using var dataSource = NpgsqlDataSource.Create(connectionString);
            string querystring = "SELECT \"ServicePointID\", \"Description\", AVG (\"TimeShowedUp\"-\"TimeQueued\") AS \"avgWaitingTime\", AVG (\"TimeFinished\"-\"TimeShowedUp\") AS \"avgServiceTime\", COUNT (\"ServicePointID\") AS \"TotalCustomers\" FROM servedcustomers INNER JOIN servicepoints ON (\"ServicePointID\" = \"ID\") WHERE \"ShowedUp\" = true GROUP BY \"ServicePointID\", \"ID\" ";
            await using var command = dataSource.CreateCommand(querystring);
            await using var reader = await command.ExecuteReaderAsync();
            var result = new List<ServicePointAnalytic>();
            while (await reader.ReadAsync())
            {
                var spa = new ServicePointAnalytic
                {
                    servicePointID = reader.GetString(0),
                    serviceDescription = reader.GetString(1),
                    avgWaitingTime = reader.GetTimeSpan(2),
                    avgServiceTime = reader.GetTimeSpan(3),
                    totalCustomers = reader.GetInt32(4)

                };
                result.Add(spa);
            }
            return result.ToArray();
        }
    }
}