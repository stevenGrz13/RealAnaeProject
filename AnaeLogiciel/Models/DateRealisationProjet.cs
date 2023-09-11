using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("daterealisationprojet")]
public class DateRealisationProjet
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idprojet")]
    [DisplayName("Projet")]
    public int IdProjet { get; set; }
    
    [Column("datedebutrealisation")]
    public DateOnly DateDebutRealisation { get; set; }
    
    [Column("datefinrealisation")]
    public DateOnly DateFinRealisation { get; set; }
    
    [ForeignKey("IdProjet")]
    public virtual Projet? Projet { get; set; }
}