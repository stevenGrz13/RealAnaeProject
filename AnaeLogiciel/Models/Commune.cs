using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("commune")]
public class Commune
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("id_district")]
    [DisplayName("district")]
    public int IdDistrict { get; set; }
    
    [Column("nom")]
    public string Nom { get; set; }
    
    [ForeignKey("IdDistrict")]
    public virtual District? District { get; set; }
}