namespace LoadBalancer.Models;

public class User
{
    public User(int id, string username, string password, string email)
    {
        ID = id;
        Username = username;
        Password = password;
        Email = email;
    }

    public virtual int ID { get; set; }
    public virtual string Username { get; set; }
    public virtual string Password { get; set; }
    public virtual string Email { get; set; }
}