using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("targettechnicienindicateuractivite")]
public class TargetTechnicienIndicateurActivite
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idtechnicien")]
    [DisplayName("technicien")]
    public int IdTechnicien { get; set; }
    
    [Column("idindicateuractivite")]
    [DisplayName("indicateuractivite")]
    public int IdIndicateurActivite { get; set; }
    
    [Column("target")]
    public double Target { get; set; }
    
    [ForeignKey("IdTechnicien")]
    public virtual Technicien? Technicien { get; set; }
    
    [ForeignKey("IdIndicateurActivite")]
    public virtual IndicateurActivite? IndicateurActivite { get; set; }
}