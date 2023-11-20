using LoadBalancer.DataBase.Connection;
using LoadBalancer.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace LoadBalancer.DataBase;

public class Context : DbContext
{

    // Tables
    public DbSet<Todo> Todo { get; set; } = null!;
    public DbSet<User> User { get; set; } = null!;
    private readonly string _connectionString = null!;

    public Context(string connectionString)
    {
        _connectionString = connectionString;
    }

    public Context() 
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Console.WriteLine("OnConfiguring");
        optionsBuilder.UseNpgsql(_connectionString);
        optionsBuilder.AddInterceptors(new TaggedQueryCommandInterceptor());
    }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<User>().HasData(
    //         new User { Id = 1, Name = "Joe", Email = "joe.doe@gmail.com", Sex = "Other" },
    //         new User { Id = 2, Name = "Mary", Email = "mary.bruh@gmail.com", Sex = "Male" }
    //     );
    // }
}