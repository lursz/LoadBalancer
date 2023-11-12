using Npgsql;

namespace LoadBalancer.db;

public class DBsHandler
{
    private Reader _reader  = new Reader();
    public DBsHandler()
    {
    }

    public void CreateTable(int dbId)
    {
        var dbInstance = _reader.DBsConnectionStrings?[dbId];
        using var connection = new NpgsqlConnection(dbInstance?.ConnectionString);
        connection.Open();

        using var command =
            new NpgsqlCommand(
                "CREATE TABLE notUsers (id serial PRIMARY KEY, username VARCHAR (50) UNIQUE NOT NULL, password VARCHAR (50) NOT NULL, email VARCHAR (355) UNIQUE NOT NULL)",
                connection);
        command.ExecuteNonQuery();
    }
}