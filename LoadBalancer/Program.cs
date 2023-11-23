using LoadBalancer.DataBase;
using LoadBalancer.DataBase.Connection;
using LoadBalancer.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace LoadBalancer;

internal static class Program
{
    private static void Main(string[] args)
    {
        // DbHandler.Create(new User { Id = 5, Name = "Ana", Email = "ana@gmail.com", Sex = "Other" });
        // DbHandler.Delete(new User { Id = 5 });
 
        var context = new Context("Host=localhost;Port=5432;Username=user1;Password=password1;Database=database1");       

        context.User.Add(new User { Name = "Ana", Email = "ana@gmail.com", Sex = "Other" });
        context.SaveChanges();
    }
}