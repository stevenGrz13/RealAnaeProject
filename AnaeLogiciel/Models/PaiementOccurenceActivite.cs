using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("paiementoccurenceactivite")]
public class PaiementOccurenceActivite
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idoccurenceactivite")]
    [DisplayName("occurenceactivite")]
    public int IdOccurenceActivite { get; set; }
    
    [Column("idtechnicien")]
    [DisplayName("technicien")]
    public int IdTechnicien { get; set; }
    
    [Column("montant")]
    public double Montant { get; set; }
    
    [Column("motif")]
    public string Motif { get; set; }
    
    [Column("dateaction")]
    public DateOnly DateAction { get; set; }
    
    [ForeignKey("IdOccurenceActivite")]
    public virtual OccurenceActivite? OccurenceActivite { get; set; }
    
    [ForeignKey("IdTechnicien")]
    public virtual Technicien? Technicien { get; set; }
}