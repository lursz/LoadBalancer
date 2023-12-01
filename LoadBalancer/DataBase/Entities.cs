using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoadBalancer.DataBase;

public class Todo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] public string Title { get; set; }

    public string? Describtion { get; set; }

    [Column(TypeName = "DATE")] public DateTime Deadline { get; set; }

    public bool Done { get; set; }
    public User? User { get; set; }
}

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] public string Name { get; set; }

    public string Email { get; set; }

    public string Sex { get; set; }

    public ICollection<Todo> Todos { get; set; }
}