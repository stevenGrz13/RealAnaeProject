using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("district")]
public class District
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("id_region")]
    [DisplayName("region")]
    public int IdRegion { get; set; }
    
    [Column("libelle")]
    public string Libelle { get; set; }
    
    [ForeignKey("IdRegion")]
    public virtual Region? Region { get; set; }
}