using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Models;

[Table("techproj")]
[Keyless]
public class TechProj
{
    [Column("idoccurence")]
    [DisplayName("occurenceactivite")]
    public int IdOccurence { get; set; }
    
    [Column("idprojet")]
    [DisplayName("projet")]
    public int IdProjet { get; set; }
    
    [Column("idtechnicien")]
    [DisplayName("technicien")]
    public int IdTechnicien { get; set; }
    
    [ForeignKey("IdOccurence")]
    public virtual OccurenceActivite? OccurenceActivite { get; set; }
    
    [ForeignKey("IdProjet")]
    public virtual Projet? Projet { get; set; }
    
    [ForeignKey("IdTechnicien")]
    public virtual Technicien? Technicien { get; set; }
}