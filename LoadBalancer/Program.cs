using LoadBalancer.DataBase;
using LoadBalancer.DataBase.Entities;

namespace LoadBalancer;

internal static class Program
{
    private static void Main(string[] args)
    {
        using var session = NHibernateHelper.OpenSession();
        using var transaction = session.BeginTransaction();
        var user = new User
        {
            Id = 1,
            Name = "John",
            Email = "john@example.com",
            Sex = "Male"
        };
        session.Save(user);
        transaction.Commit();

        // var users_query = session.CreateQuery("FROM User");
        // var users = users_query.List<User>();
        // Console.WriteLine("\nwynik zapytania: ");
        // foreach (var u in users)
        // {
        //     Console.WriteLine(u.Id.ToString() + " " + u.Name + " " + u.Email + " " + u.Sex);
        // }
    }
}