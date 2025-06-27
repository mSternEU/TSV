using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TSV.Models.Base;

[Table("team")]
public partial class Team : ModelBase
{
    private int _id;
    private string _vorname;
    private string _nachname;
    private string _telefon;
    private int _funktion;
    private DateTime _createdAt;
    private bool _istAktiv;
    private DateTime? _geloeschtAm;
    private int? _geloeschtVon;

    // Navigation Properties
    private MitarbeiterFunktion _mitarbeiterFunktion;
    private List<Kurs> _kurse;

    [Key]
    [Column("id")]
    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    [Required]
    [MaxLength(100)]
    [Column("vorname")]
    public string Vorname
    {
        get => _vorname;
        set => SetProperty(ref _vorname, value);
    }

    [Required]
    [MaxLength(100)]
    [Column("nachname")]
    public string Nachname
    {
        get => _nachname;
        set => SetProperty(ref _nachname, value);
    }

    [MaxLength(50)]
    [Column("telefon")]
    public string Telefon
    {
        get => _telefon;
        set => SetProperty(ref _telefon, value);
    }

    [Required]
    [Column("funktion")]
    public int Funktion
    {
        get => _funktion;
        set => SetProperty(ref _funktion, value);
    }

    [Column("created_at")]
    public DateTime CreatedAt
    {
        get => _createdAt;
        set => SetProperty(ref _createdAt, value);
    }

    [Column("ist_aktiv")]
    public bool IstAktiv
    {
        get => _istAktiv;
        set => SetProperty(ref _istAktiv, value);
    }

    [Column("geloescht_am")]
    public DateTime? GeloeschtAm
    {
        get => _geloeschtAm;
        set => SetProperty(ref _geloeschtAm, value);
    }

    [Column("geloescht_von")]
    public int? GeloeschtVon
    {
        get => _geloeschtVon;
        set => SetProperty(ref _geloeschtVon, value);
    }

    // Navigation Properties
    [ForeignKey("Funktion")]
    public MitarbeiterFunktion MitarbeiterFunktion
    {
        get => _mitarbeiterFunktion;
        set => SetProperty(ref _mitarbeiterFunktion, value);
    }

    public List<Kurs> Kurse
    {
        get => _kurse ??= new List<Kurs>();
        set => SetProperty(ref _kurse, value);
    }

    // Computed Properties
    [NotMapped]
    public string VollName => $"{Vorname} {Nachname}";

    [NotMapped]
    public string DisplayName => $"{VollName} ({MitarbeiterFunktion?.Bezeichnung})";

    // Konstruktor
    public Team()
    {
        CreatedAt = DateTime.Now;
        IstAktiv = true;
    }
}