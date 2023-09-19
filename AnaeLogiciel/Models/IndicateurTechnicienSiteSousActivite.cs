using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("indicateurtechniciensitesousactivite")]
public class IndicateurTechnicienSiteSousActivite
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idsitesousactivite")]
    [DisplayName("sitesousactivite")]
    public int IdSiteSousActivite { get; set; }
    
    [Column("idindicateur")]
    [DisplayName("typeindicateur")]
    public int IdIndicateur { get; set; }
    
    [Column("idtechnicien")]
    [DisplayName("technicien")]
    public int IdTechnicien { get; set; }
    
    [Column("target")]
    public double Target { get; set; }
    
    [ForeignKey("IdSiteSousActivite")]
    public virtual SiteSousActivite? SiteSousActivite { get; set; }
    
    [ForeignKey("IdIndicateur")]
    public virtual TypeIndicateur? TypeIndicateur { get; set; }
    
    [ForeignKey("IdTechnicien")]
    public virtual Technicien? Technicien { get; set; }
}