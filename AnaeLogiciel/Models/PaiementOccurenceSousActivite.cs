using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("paiementoccurencesousactivite")]
public class PaiementOccurenceSousActivite
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idoccurencesousactivite")]
    [DisplayName("occurencesousactivite")]
    public int IdOccurenceSousActivite { get; set; }
    
    [Column("idtechnicien")]
    [DisplayName("technicien")]
    public int IdTechnicien { get; set; }
    
    [Column("montant")]
    public double Montant { get; set; }
    
    [Column("motif")]
    public string Motif { get; set; }
    
    [Column("dateaction")]
    public DateOnly DateAction { get; set; }
    
    [ForeignKey("IdOccurenceSousActivite")]
    public virtual OccurenceSousActivite? OccurenceSousActivite { get; set; }
    
    [ForeignKey("IdTechnicien")]
    public virtual Technicien? Technicien { get; set; }
}