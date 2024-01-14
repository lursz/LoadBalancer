namespace LoadBalancer.DataBase.Entities;

public class User
{
    public virtual int Id { get; set; }
    public virtual string Name { get; set; }
    public virtual string Email { get; set; }
    public virtual string Sex { get; set; }

    public virtual ICollection<Todo> Todos { get; set; }
}