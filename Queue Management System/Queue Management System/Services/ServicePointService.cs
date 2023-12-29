using Queue_Management_System.Models;
using Npgsql;


namespace Queue_Management_System.Services
{
    public class ServicePointService
    {
        private readonly string _connectionString;

        public ServicePointService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<ServicePoint> getServicePointbyID(string id)
        {
            var connectionString = _connectionString;
            await using var dataSource = NpgsqlDataSource.Create(connectionString);
            string querystring = $"SELECT * FROM servicepoints WHERE \"ID\"='{id}'";
            await using var command = dataSource.CreateCommand(querystring);
            await using var reader = await command.ExecuteReaderAsync();
            ServicePoint sp = new ServicePoint();
            while (await reader.ReadAsync())
            {
                sp = new ServicePoint
                {
                    ServicePointID = reader.GetString(0),
                    ServiceDescription = reader.GetString(1),
                    PassKey = reader.GetString(2),
                };
                
            }
            return sp;
        }

        public async Task<ServicePoint[]> getServicePointsDb()
        {
            var connectionString = _connectionString;
            await using var dataSource = NpgsqlDataSource.Create(connectionString);
            await using var command = dataSource.CreateCommand("SELECT * FROM servicepoints");
            await using var reader = await command.ExecuteReaderAsync();
            var result = new List<ServicePoint>();
            while (await reader.ReadAsync())
            {
                var sp = new ServicePoint
                {
                    ServicePointID = reader.GetString(0),
                    ServiceDescription = reader.GetString(1),
                    PassKey = reader.GetString(2),
                };
                result.Add(sp);
            }
            return result.ToArray();
        }

        public async void AddServicePoint(ServicePoint sp)
        {
            var connectionString = _connectionString;
            await using var dataSource = NpgsqlDataSource.Create(connectionString);

            string id = sp.ServicePointID;
            string description = sp.ServiceDescription;
            string passkey = sp.PassKey;

            string querystring = $"INSERT INTO servicepoints (\"ID\", \"Description\", \"PassKey\") VALUES ('{id}', '{description}', '{passkey}')";
            await using var command = dataSource.CreateCommand(querystring);
            await command.ExecuteNonQueryAsync();
        }

        public async void EditServicePoint(ServicePoint sp)
        {
            var connectionString = _connectionString;
            await using var dataSource = NpgsqlDataSource.Create(connectionString);

            string id = sp.ServicePointID;
            string description = sp.ServiceDescription;
            string passkey = sp.PassKey;

            string querystring = $"UPDATE servicepoints SET (\"Description\", \"PassKey\") = ('{description}', '{passkey}') WHERE \"ID\" = '{id}'";
            await using var command = dataSource.CreateCommand(querystring);
            await command.ExecuteNonQueryAsync();
        }

        public async void DeleteServicePoint(ServicePoint sp)
        {
            var connectionString = _connectionString;
            await using var dataSource = NpgsqlDataSource.Create(connectionString);

            string id = sp.ServicePointID;
            string description = sp.ServiceDescription;
            string passkey = sp.PassKey;

            string querystring = $"DELETE FROM servicepoints WHERE \"ID\"='{id}'";
            await using var command = dataSource.CreateCommand(querystring);
            await command.ExecuteNonQueryAsync();
        }
    }
    
}