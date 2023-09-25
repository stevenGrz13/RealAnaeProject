using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("occurenceactivitesource")]
public class OccurenceActiviteSource
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idoccurenceactivite")]
    [DisplayName("occurenceactivite")]
    public int IdOccurenceActivite { get; set; }
    
    [Column("idsource")]
    [DisplayName("sourcedeverification")]
    public int IdSource { get; set; }
    
    [ForeignKey("IdOccurenceActivite")]
    public virtual OccurenceActivite? OccurenceActivite { get; set; }
    
    [ForeignKey("IdSource")]
    public virtual SourceDeVerification? SourceDeVerification { get; set; }
}