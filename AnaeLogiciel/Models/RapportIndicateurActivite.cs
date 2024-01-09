using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("rapportindicateuractivite")]
public class RapportIndicateurActivite
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
    
    [Column("quantiteeffectue")]
    public double QuantiteEffectue { get; set; }
    
    [Column("datedebut")]
    public DateOnly DateDebut { get; set; }
    
    [Column("datefin")]
    public DateOnly DateFin { get; set; }

    [NotMapped]
    public string Couleur { get; set; }
    
    [ForeignKey("IdTechnicien")]
    public virtual Technicien? Technicien { get; set; }
    
    [ForeignKey("IdIndicateurActivite")]
    public virtual IndicateurActivite? IndicateurActivite { get; set; }
}