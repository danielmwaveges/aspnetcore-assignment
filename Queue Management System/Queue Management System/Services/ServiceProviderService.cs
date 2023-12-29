using Queue_Management_System.Models;
using Npgsql;

namespace Queue_Management_System.Services
{
    public class ServiceProviderService
    {
        private readonly string _connectionString;

        public ServiceProviderService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Models.ServiceProvider> getServiceProviderbyEmail(string email)
        {
            var connectionString = _connectionString;
            await using var dataSource = NpgsqlDataSource.Create(connectionString);
            string querystring = $"SELECT * FROM serviceproviders WHERE \"Email\"='{email}'";
            await using var command = dataSource.CreateCommand(querystring);
            await using var reader = await command.ExecuteReaderAsync();
            Models.ServiceProvider provider = new Models.ServiceProvider();
            while (await reader.ReadAsync())
            {
                provider.id = reader.GetInt32(0);
                provider.name = reader.GetString(1);
                provider.email = reader.GetString(2);
                provider.password = reader.GetString(3);
                provider.isAdmin = reader.GetBoolean(4);
            }
            return provider;

        }
    }
}