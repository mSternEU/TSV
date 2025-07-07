using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TSV.Models.Base;
using TSV.Models.Business;

[Table("kunde_rolle")]
public partial class KundeRolle : ModelBase
{
    private int _id;
    private string _bezeichnung;

    [Key]
    [Column("id")]
    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    [Required]
    [MaxLength(100)]
    [Column("bezeichnung")]
    public string Bezeichnung
    {
        get => _bezeichnung;
        set => SetProperty(ref _bezeichnung, value);
    }

    // Navigation Properties
    public List<Kunde> Kunden { get; set; } = new List<Kunde>();
}