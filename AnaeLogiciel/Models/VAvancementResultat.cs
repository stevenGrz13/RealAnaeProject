using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Models;

[Table("vavancementresultat")]
[Keyless]
public class VAvancementResultat
{
    [Column("idoccurenceresultat")]
    [DisplayName("occurenceresultat")]
    public int IdResultat { get; set; }
    
    [Column("avancement")]
    public double Avancement { get; set; }
    
    [ForeignKey("IdResultat")]
    public virtual OccurenceResultat? OccurenceResultat { get; set; }
}