using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Models;

[Table("vcalculrapportactivite")]
[Keyless]
public class VCalculRapportActivite
{
    [Column("idoccurenceactivite")]
    [DisplayName("occurenceactivite")]
    public int IdOccurenceActivite { get; set; }
    
    [Column("idindicateur")]
    [DisplayName("IdIndicateur")]
    public int IdIndicateur { get; set; }
    
    [Column("sommetotale")]
    public double Somme { get; set; }
    
    [ForeignKey("IdOccurenceActivite")]
    public virtual OccurenceActivite? OccurenceActivite { get; set; }
    
    [ForeignKey("IdIndicateur")]
    public virtual TypeIndicateur? TypeIndicateur { get; set; }
}