using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("associationindicateuractivite")]
public class AssociationIndicateurActivite
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idoccurence")]
    [DisplayName("occurenceactivite")]
    public int IdOccurence { get; set; }
    
    [Column("idtypeindicateur")]
    [DisplayName("typeindicateur")]
    public int IdTypeIndicateur { get; set; }
    
    [Column("quantitedemande")]
    public double QuantiteDemande { get; set; }
    
    [Column("finishedornot")]
    public bool FinishedOrNot { get; set; }
    
    [ForeignKey("IdTypeIndicateur")]
    public virtual TypeIndicateur? TypeIndicateur { get; set; }
    
    [ForeignKey("IdOccurence")]
    public virtual OccurenceActivite? OccurenceActivite { get; set; }
}