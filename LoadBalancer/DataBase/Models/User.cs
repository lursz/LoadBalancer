using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoadBalancer.DataBase.Models;


public class User 
{
    [Key]
    public int Id { get; set; }

    [Required] 
    public string Name { get; set; }
 
    public string Email { get; set; }

    public string Sex { get; set; }
    
    public ICollection<Todo> Todos {get; set;} 
    
}