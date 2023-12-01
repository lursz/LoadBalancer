using FluentNHibernate.Mapping;

namespace LoadBalancer.DataBase;

public class TodoMap : ClassMap<Todo>
{
    public TodoMap()
    {
        Id(x => x.Id);
        Map(x => x.Title).Not.Nullable();
        Map(x => x.Describtion);
        Map(x => x.Deadline).CustomSqlType("DATE");
        Map(x => x.Done);
        References(x => x.User).Cascade.All();
    }
}

public class UserMap : ClassMap<User>
{
    public UserMap()
    {
        Id(x => x.Id);
        Map(x => x.Name).Not.Nullable();
        Map(x => x.Email);
        Map(x => x.Sex);
        HasMany(x => x.Todos).Inverse().Cascade.All();
    }
}