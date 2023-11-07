using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("targettechnicienindicateursousactivite")]
public class TargetTechnicienIndicateurSousActivite
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idtechnicien")]
    [DisplayName("technicien")]
    public int IdTechnicien { get; set; }
    
    [Column("idindicateursousactivite")]
    [DisplayName("indicateursousactivite")]
    public int IdIndicateurSousActivite { get; set; }
    
    [Column("target")]
    public double Target { get; set; }
    
    [ForeignKey("IdTechnicien")]
    public virtual Technicien? Technicien { get; set; }
    
    [ForeignKey("IdIndicateurSousActivite")]
    public virtual IndicateurSousActivite? IndicateurSousActivite { get; set; }
}