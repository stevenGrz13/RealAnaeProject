using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("technicienentitysousactivite")]
public class TechnicienEntitySousActivite
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idtechnicien")]
    [DisplayName("technicien")]
    public int IdTechnicien { get; set; }
    
    [Column("idoccurencesousactivite")]
    [DisplayName("occurencesousactivite")]
    public int IdOccurenceSousActivite { get; set; }
    
    [Column("identity")]
    [DisplayName("entity")]
    public int IdEntity { get; set; }
    
    [Column("quantite")]
    public int Quantite { get; set; }
    
    [ForeignKey("IdTechnicien")]
    public virtual Technicien? Technicien { get; set; }
    
    [ForeignKey("IdOccurenceSousActivite")]
    public virtual OccurenceSousActivite? OccurenceSousActivite { get; set; }
    
    [ForeignKey("IdEntity")]
    public virtual Entity? Entity { get; set; }
}