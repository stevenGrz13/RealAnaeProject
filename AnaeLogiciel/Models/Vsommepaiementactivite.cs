using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Models;

[Table("vsommepaiementactivite")]
[Keyless]
public class Vsommepaiementactivite
{
    [Column("idoccurenceactivite")]
    [DisplayName("occurenceactivite")]
    public int IdOccurenceActivite { get; set; }
    
    [Column("idoccurenceresultat")]
    [DisplayName("occurenceresultat")]
    public int IdOccurenceResultat { get; set; }
    
    [Column("somme")]
    public double Somme { get; set; }
    
    [ForeignKey("IdOccurenceActivite")]
    public virtual OccurenceActivite? OccurenceActivite { get; set; }
    
    [ForeignKey("IdOccurenceResultat")]
    public virtual OccurenceResultat? OccurenceResultat { get; set; }
}