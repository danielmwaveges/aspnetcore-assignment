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
        
        public async Task<Models.ServiceProvider[]> getServiceProviders()
        {
            var connectionString = _connectionString;
            await using var dataSource = NpgsqlDataSource.Create(connectionString);
            string querystring = "SELECT * FROM serviceproviders";
            await using var command = dataSource.CreateCommand(querystring);
            await using var reader = await command.ExecuteReaderAsync();
            var result = new List<Models.ServiceProvider>();
            while (await reader.ReadAsync())
            {
                var provider = new Models.ServiceProvider
                {
                    id = reader.GetInt32(0),
                    name = reader.GetString(1),
                    email = reader.GetString(2),
                    password = reader.GetString(3),
                    isAdmin = reader.GetBoolean(4),
                };
                result.Add(provider);
            }
            return result.ToArray();
        
        }
        
        public async void AddServiceProvider(Models.ServiceProvider provider)
        {
            var connectionString = _connectionString;
            await using var dataSource = NpgsqlDataSource.Create(connectionString);

            //string id = provider.id;
            string name = provider.name;
            string email = provider.email;
            string password = provider.password;
            bool isAdmin = provider.isAdmin;

            string querystring = $"INSERT INTO serviceproviders (\"Name\", \"Email\", \"Password\", \"IsAdmin\") VALUES ('{name}', '{email}', '{password}', '{isAdmin}')";
            await using var command = dataSource.CreateCommand(querystring);
            await command.ExecuteNonQueryAsync();
        }   

        public async void EditServiceProvider(Models.ServiceProvider provider)
        {
            var connectionString = _connectionString;
            await using var dataSource = NpgsqlDataSource.Create(connectionString);

            int id = provider.id;
            string name = provider.name;
            string email = provider.email;
            string password = provider.password;
            bool isAdmin = provider.isAdmin;

            string querystring = $"UPDATE serviceproviders SET (\"Name\", \"Email\", \"Password\", \"IsAdmin\") = ('{name}', '{email}', '{password}', '{isAdmin}') WHERE \"ID\" = '{id}'";
            await using var command = dataSource.CreateCommand(querystring);
            await command.ExecuteNonQueryAsync();
        }

        public async void DeleteServiceProvider(Models.ServiceProvider provider)
        {
            var connectionString = _connectionString;
            await using var dataSource = NpgsqlDataSource.Create(connectionString);

            int id = provider.id;

            string querystring = $"DELETE FROM serviceproviders WHERE \"ID\"='{id}'";
            await using var command = dataSource.CreateCommand(querystring);
            await command.ExecuteNonQueryAsync();
        }
    }
}