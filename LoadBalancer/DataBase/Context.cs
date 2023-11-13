using LoadBalancer.DataBase.Connection;
using LoadBalancer.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace LoadBalancer.DataBase;

public class Context : DbContext
{
    private Reader Reader { get; } = new();
    
    // Tables
    public DbSet<Todo> Todo { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Console.WriteLine("OnConfiguring");
        optionsBuilder.UseNpgsql(Reader.DBsConnectionStrings?[0].ConnectionString);
    }
}