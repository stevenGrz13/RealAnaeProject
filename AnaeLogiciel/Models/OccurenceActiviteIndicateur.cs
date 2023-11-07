using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("occurenceactiviteindicateur")]
public class OccurenceActiviteIndicateur
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idoccurenceactivite")]
    [DisplayName("occurenceactivite")]
    public int IdOccurenceActivite { get; set; }
    
    [Column("idindicateuractivite")]
    [DisplayName("indicateuractivite")]
    public int IdIndicateurActivite { get; set; }
    
    [Column("target")]
    public double Target { get; set; }
    
    [ForeignKey("IdOccurenceActivite")]
    public virtual OccurenceActivite? OccurenceActivite { get; set; }
    
    [ForeignKey("IdIndicateurActivite")]
    public virtual IndicateurActivite? IndicateurActivite { get; set; }
}