using System.Text.Json;
using Npgsql;

namespace LoadBalancer.db;

public class DBHandler
{
    public static List<DBInstance> DbInstances { get; set; }
    private readonly string _connectionString;


    public DBHandler(string connectionString)
    {
        _connectionString = connectionString;
    }

    private static void GetConnectionStringList()
    {
        var jsonContent = File.ReadAllText("db/db.json");
        DbInstances = JsonSerializer.Deserialize<List<DBInstance>>(jsonContent) ?? throw new InvalidOperationException();
        // return JsonSerializer.Deserialize<List<DBInstance>>(jsonContent) ?? throw new InvalidOperationException();
    }

    public void CreateTable()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var command =
            new NpgsqlCommand(
                "CREATE TABLE notUsers (id serial PRIMARY KEY, username VARCHAR (50) UNIQUE NOT NULL, password VARCHAR (50) NOT NULL, email VARCHAR (355) UNIQUE NOT NULL)",
                connection);
        command.ExecuteNonQuery();
    }
}