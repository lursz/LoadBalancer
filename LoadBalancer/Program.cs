using LoadBalancer.DataBase;
using LoadBalancer.DataBase.Connection;
using LoadBalancer.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace LoadBalancer;

internal static class Program
{
    private static void Main(string[] args)
    {
        // DbHandler.Create(new User { Id = 5, Name = "Ana", Email = "ana@gmail.com", Sex = "Other" });
        // DbHandler.Delete(new User { Id = 5 });

        var context = new Context("Host=localhost;Port=5432;Username=user1;Password=password1;Database=database1");
        context.User.Add()
    }
}