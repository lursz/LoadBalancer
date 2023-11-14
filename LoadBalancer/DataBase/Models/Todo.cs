using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoadBalancer.DataBase.Models;

public class Todo
{
    [Key]
    public int Id { get; set; }

    [Required] 
    public required string Title { get; set; }

    public string? Describtion { get; set; }

    [Column(TypeName = "DATE")] 
    public DateTime Deadline { get; set; }

    public bool Done { get; set; }
    public User? User { get; set;}
}