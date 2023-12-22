namespace LoadBalancer.DataBase.Entities;

public class Todo
{
    public virtual int Id { get; set; }
    public virtual string Title { get; set; }
    public virtual string? Description { get; set; }
    public virtual DateTime Deadline { get; set; }
    public virtual bool Done { get; set; }
    public virtual User? Owner { get; set; }
}