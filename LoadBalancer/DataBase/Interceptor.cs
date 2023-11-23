

using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;


class ReadLoadBalancerInterceptor : DbCommandInterceptor
{
    // private readonly string[] readConnectionStrings;

    public ReadLoadBalancerInterceptor()
    {
        // this.readConnectionStrings = readConnectionStrings;
    }

    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {

        // command.Connection!.ConnectionString = "Host=localhost;Port=5433;Username=user2;Password=password2;Database=database2";
        command.Connection.ChangeDatabase("database2");
        Console.WriteLine("Interceptor");
        Console.WriteLine("command text: " + command.CommandText);
        Console.WriteLine("connection string: " + command.Connection!.ConnectionString);

        return result;
    }
}

class ConnectionInterceptor : DbConnectionInterceptor
{
    public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
    {
        Console.WriteLine("Connection opened");
        Console.WriteLine("connection string: " + connection.ConnectionString);
       
    }
}