using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("partenairetechnique")]
public class PartenaireTechnique
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("nom")]
    public string Nom { get; set; }
}