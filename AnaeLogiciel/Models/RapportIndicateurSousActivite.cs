using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("rapportindicateursousactivite")]
public class RapportIndicateurSousActivite
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
    
    [Column("quantiteeffectue")]
    public double QuantiteEffectue { get; set; }
    
    [Column("datedebut")]
    public DateOnly DateDebut { get; set; }
    
    [Column("datefin")]
    public DateOnly DateFin { get; set; }
    
    [ForeignKey("IdTechnicien")]
    public virtual Technicien? Technicien { get; set; }
    
    [ForeignKey("IdIndicateurSousActivite")]
    public virtual IndicateurSousActivite? IndicateurSousActivite { get; set; }
}