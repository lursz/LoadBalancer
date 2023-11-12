using Npgsql;

namespace LoadBalancer.db;

public class DBsHandler
{
    public Reader Reader { get; } = new();

    public void CreateTable(int dbId)
    {
        var dbInstance = Reader.DBsConnectionStrings?[dbId];
        using var connection = new NpgsqlConnection(dbInstance?.ConnectionString);
        connection.Open();

        using var command =
            new NpgsqlCommand(
                "CREATE TABLE notUsers (id serial PRIMARY KEY, username VARCHAR (50) UNIQUE NOT NULL, password VARCHAR (50) NOT NULL, email VARCHAR (355) UNIQUE NOT NULL)",
                connection);
        command.ExecuteNonQuery();
    }
    
    public void removeTable (string tableName, int dbId)
    {
        var dbInstance = Reader.DBsConnectionStrings?[dbId];
        using var connection = new NpgsqlConnection(dbInstance?.ConnectionString);
        Console.WriteLine(dbInstance.ConnectionString);
        connection.Open();

        using var command =
            new NpgsqlCommand(
                $"DROP TABLE {tableName}",
                connection);
        command.ExecuteNonQuery();
    }
}