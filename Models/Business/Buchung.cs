using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TSV.Models.Base;
using TSV.Models.Business;

[Table("buchung")]
public partial class Buchung : ModelBase
{
    private int _bid;
    private DateTime _bDatum;
    private int _kursId;
    private int _p1;
    private int _p1Role;
    private int? _p2;
    private int? _p2Role;
    private int _bezahlt;
    private DateTime _erstelltAm;
    private bool _istAktiv;
    private DateTime? _geloeschtAm;
    private int? _geloeschtVon;

    // Navigation Properties
    private Kunde _kundeP1;
    private Kunde _kundeP2;
    private Kurs _kurs;
    private KundeRolle _p1Rolle;
    private KundeRolle _p2Rolle;

    [Key]
    [Column("bid")]
    public int Bid
    {
        get => _bid;
        set => SetProperty(ref _bid, value);
    }

    [Required]
    [Column("b_datum")]
    public DateTime BDatum
    {
        get => _bDatum;
        set => SetProperty(ref _bDatum, value);
    }

    [Required]
    [Column("kurs_id")]
    public int KursId
    {
        get => _kursId;
        set => SetProperty(ref _kursId, value);
    }

    [Required]
    [Column("p1")]
    public int P1
    {
        get => _p1;
        set => SetProperty(ref _p1, value);
    }

    [Required]
    [Column("p1_role")]
    public int P1Role
    {
        get => _p1Role;
        set => SetProperty(ref _p1Role, value);
    }

    [Column("p2")]
    public int? P2
    {
        get => _p2;
        set => SetProperty(ref _p2, value);
    }

    [Column("p2_role")]
    public int? P2Role
    {
        get => _p2Role;
        set => SetProperty(ref _p2Role, value);
    }

    [Column("bezahlt")]
    public int Bezahlt
    {
        get => _bezahlt;
        set => SetProperty(ref _bezahlt, value);
    }

    [Column("created_at")]
    public DateTime ErstelltAm
    {
        get => _erstelltAm;
        set => SetProperty(ref _erstelltAm, value);
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
    [ForeignKey("P1")]
    public Kunde KundeP1
    {
        get => _kundeP1;
        set => SetProperty(ref _kundeP1, value);
    }

    [ForeignKey("P2")]
    public Kunde KundeP2
    {
        get => _kundeP2;
        set => SetProperty(ref _kundeP2, value);
    }

    [ForeignKey("KursId")]
    public Kurs Kurs
    {
        get => _kurs;
        set => SetProperty(ref _kurs, value);
    }

    // Computed Properties
    [NotMapped]
    public string BezahlStatus
    {
        get
        {
            return Bezahlt switch
            {
                0 => "Offen",
                1 => "P1 bezahlt",
                2 => "P2 bezahlt",
                3 => "Vollständig bezahlt",
                _ => "Unbekannt"
            };
        }
    }

    // Konstruktor
    public Buchung()
    {
        ErstelltAm = DateTime.Now;
        IstAktiv = true;
        Bezahlt = 0;
    }
}