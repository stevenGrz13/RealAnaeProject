using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("technicienprojet")]
public class TechnicienProjet
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idtechnicien")]
    [DisplayName("technicien")]
    public int IdTechnicien { get; set; }
    
    [Column("idprojet")]
    [DisplayName("projet")]
    public int IdProjet { get; set; }
    
    [Column("isfinished")]
    public bool isFinished { get; set; }
    
    [ForeignKey("IdTechnicien")]
    public virtual Technicien? Technicien { get; set; }
    
    [ForeignKey("IdProjet")]
    public virtual Projet? Projet { get; set; }
}