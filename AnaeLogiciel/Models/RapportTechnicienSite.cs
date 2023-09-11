using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("rapporttechniciensite")]
public class RapportTechnicienSite
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idsite")]
    [DisplayName("site")]
    public int IdSite { get; set; }
    
    [Column("idtechnicien")]
    [DisplayName("technicien")]
    public int IdTechnicien { get; set; }
    
    [Column("idindicateur")]
    [DisplayName("indicateur")]
    public int IdIndicateur { get; set; }
    
    [Column("targeteffectue")]
    public double TargetEffectue { get; set; }
    
    [Column("dateaction")]
    public DateOnly DateAction { get; set; }
    
    [ForeignKey("IdTechnicien")]
    public virtual Technicien? Technicien { get; set; }
    
    [ForeignKey("IdIndicateur")]
    public virtual TypeIndicateur? TypeIndicateur { get; set; }
    
    [ForeignKey("IdSite")]
    public virtual Site? Site { get; set; }
}