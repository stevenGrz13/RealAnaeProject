using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("occurencesourcedeverification")]
public class OccurenceSourceDeVerification
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idoccurence")]
    [DisplayName("occurenceactivite")]
    public int IdOccurence { get; set; }
    
    [Column("idsourcedeverification")]
    [DisplayName("sourcedeverification")]
    public int IdSourcedeverification { get; set; }

    [ForeignKey("IdOccurence")]
    public virtual OccurenceActivite? OccurenceActivite { get; set; }
    
    [ForeignKey("IdSourcedeverification")]
    public virtual SourceDeVerification? SourceDeVerification { get; set; }
}