using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("admin")]
public class Admin
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("email")]
    public string Email { get; set; }
    
    [Column("pass")]
    public string Pass { get; set; }
    
    [Column("token")]
    public string Token { get; set; }
}