using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("projet")]
public class Projet
{
    [Key] 
    [Column("id")]
    public int Id { get; set; }
    
    [Column("nom")]
    public string Nom { get; set; }
    
    [Column("datedebutprevision")]
    public DateOnly DateDebutPrevision { get; set; }
    
    [Column("datefinprevision")]
    public DateOnly DateFinPrevision { get; set; }
    
    [Column("finishedornot")]
    public bool FinishedOrNot { get; set; }
    
    [Column("avancement")]
    public double Avancement { get; set; }
}