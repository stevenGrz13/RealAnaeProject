using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Models;

[Table("vcalculrapportsousactivite")]
[Keyless]
public class VCalculRapportSousActivite
{
    [Column("idoccurencesousactivite")]
    [DisplayName("occurencesousactivite")]
    public int IdOccurenceSousActivite { get; set; }
    
    [Column("idindicateur")]
    [DisplayName("IdIndicateur")]
    public int IdIndicateur { get; set; }
    
    [Column("sommetotale")]
    public double Somme { get; set; }
    
    [ForeignKey("IdOccurenceSousActivite")]
    public virtual OccurenceSousActivite? OccurenceSousActivite { get; set; }
    
    [ForeignKey("IdIndicateur")]
    public virtual TypeIndicateur? TypeIndicateur { get; set; }
}