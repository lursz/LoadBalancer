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
    }
}