using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("occurenceactivitesourcetechnicien")]
public class OccurenceActiviteSourceTechnicien
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
    
    [Column("idtechnicien")]
    [DisplayName("technicien")]
    public int IdTechnicien { get; set; }
    
    [Column("lienfichier")]
    public string LienFichier { get; set; }
    
    [Column("dateaction")]
    public DateOnly DateAction { get; set; }
    
    [ForeignKey("IdOccurenceActivite")]
    public virtual OccurenceActivite? OccurenceActivite { get; set; }
    
    [ForeignKey("IdSource")]
    public virtual SourceDeVerification? SourceDeVerification { get; set; }
    
    [ForeignKey("IdTechnicien")]
    public virtual Technicien? Technicien { get; set; }
}