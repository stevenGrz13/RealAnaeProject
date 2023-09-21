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
    
    [Column("idsiteactivite")]
    [DisplayName("siteactivite")]
    public int IdSiteActivite { get; set; }
    
    [Column("idindicateur")]
    [DisplayName("typeindicateur")]
    public int IdIndicateur { get; set; }
    
    [Column("quantite")]
    public double Quantite { get; set; }
    
    [Column("dateaction")]
    public DateOnly DateAction { get; set; }
    
    [ForeignKey("IdTechnicien")]
    public virtual Technicien? Technicien { get; set; }
    
    [ForeignKey("IdSiteActivite")]
    public virtual SiteActivite? SiteActivite { get; set; }
    
    [ForeignKey("IdIndicateur")]
    public virtual TypeIndicateur? TypeIndicateur { get; set; }
}