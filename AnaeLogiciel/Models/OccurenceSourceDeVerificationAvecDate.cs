using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("occurencesourcedeverificationavecdate")]
public class OccurenceSourceDeVerificationAvecDate
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idoccurence")]
    [DisplayName("occurenceactivite")]
    public int IdOccurence { get; set; }
    
    [Column("idtechnicien")]
    [DisplayName("technicien")]
    public int IdTechnicien { get; set; }
    
    [Column("idsourcedeverification")]
    [DisplayName("sourcedeverification")]
    public int IdSourcedeverification { get; set; }
    
    [Column("dateaction")]
    public DateOnly DateAction { get; set; }
    
    [Column("fichier")]
    public string Fichier { get; set; }

    [ForeignKey("IdTechnicien")]
    public virtual Technicien? Technicien { get; set; }
    
    [ForeignKey("IdSourcedeverification")]
    public virtual SourceDeVerification? SourceDeVerification { get; set; }
    
    [ForeignKey("IdOccurence")]
    public virtual OccurenceActivite? OccurenceActivite { get; set; }
}