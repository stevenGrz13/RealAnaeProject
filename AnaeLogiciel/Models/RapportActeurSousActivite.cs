using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("rapportacteursousactivite")]
public class RapportActeurSousActivite
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idacteur")]
    [DisplayName("acteur")]
    public int IdActeur { get; set; }
    
    [Column("idindicateursousactivite")]
    [DisplayName("indicateursousactivite")]
    public int IdIndicateurSousActivite { get; set; }
    
    [Column("quantiteeffectue")]
    public double QuantiteEffectue { get; set; }
    
    [Column("datedebut")]
    public DateOnly DateDebut { get; set; }
    
    [Column("datefin")]
    public DateOnly DateFin { get; set; }
    
    [ForeignKey("IdActeur")]
    public virtual Acteur? Acteur { get; set; }
    
    [ForeignKey("IdIndicateurSousActivite")]
    public virtual IndicateurSousActivite? IndicateurSousActivite { get; set; }
}