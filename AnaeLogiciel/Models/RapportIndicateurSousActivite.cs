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
    
    [Column("idsitesousactivite")]
    [DisplayName("sitesousactivite")]
    public int IdSiteSousActivite { get; set; }
    
    [Column("idindicateur")]
    [DisplayName("typeindicateur")]
    public int IdIndicateur { get; set; }
    
    [Column("quantite")]
    public double Quantite { get; set; }
    
    [Column("dateaction")]
    public DateOnly DateAction { get; set; }
    
    [ForeignKey("IdTechnicien")]
    public virtual Technicien? Technicien { get; set; }
    
    [ForeignKey("IdSiteSousActivite")]
    public virtual SiteSousActivite? SitesousActivite { get; set; }
    
    [ForeignKey("IdIndicateur")]
    public virtual TypeIndicateur? TypeIndicateur { get; set; }
}