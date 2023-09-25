using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Models;

[Table("vlienactivitesousactivite")]
[Keyless]
public class VLienActiviteSousActivite
{
    [Column("idoccurenceactivite")]
    [DisplayName("occurenceactivite")]
    public int IdOccurenceActivite { get; set; }
    
    [Column("idoccurencesousactivite")]
    [DisplayName("occurencesousactivite")]
    public int IdOccurenceSousActivite { get; set; }
    
    [Column("avancement")]
    public double Avancement { get; set; }
    
    [ForeignKey("IdOccurenceActivite")]
    public virtual OccurenceActivite? OccurenceActivite { get; set; }
    
    [ForeignKey("IdOccurenceSousActivite")]
    public virtual OccurenceSousActivite? OccurenceSousActivite { get; set; }
}