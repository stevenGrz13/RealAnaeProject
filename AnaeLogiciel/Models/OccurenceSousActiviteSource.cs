using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("occurencesousactivitesource")]
public class OccurenceSousActiviteSource
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idoccurencesousactivite")]
    [DisplayName("occurencesousactivite")]
    public int IdOccurenceSousActivite { get; set; }
    
    [Column("idsource")]
    [DisplayName("sourcedeverification")]
    public int IdSource { get; set; }
    
    [ForeignKey("IdOccurenceSousActivite")]
    public virtual OccurenceSousActivite? OccurenceSousActivite { get; set; }
    
    [ForeignKey("IdSource")]
    public virtual SourceDeVerification? SourceDeVerification { get; set; }
}