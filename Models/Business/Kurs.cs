using TSV.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSV.Models.Business
{
    [Table("kurse")]
    public class Kurs : ModelBase
    {
        private int _id;
        private string _kursName;
        private string _beschreibung;
        private decimal _preis;
        private int _maxTeilnehmer;
        private DateTime _startDatum;
        private DateTime? _endDatum;
        private string _wochentag;
        private TimeSpan? _uhrzeit;
        private int _anzahlTermine;
        private int? _lehrerId;
        private bool _istWorkshop;

        // Navigation Properties
        private Team _lehrer;
        private List<Buchung> _buchungen;

        [Key]
        [Column("id")]
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        [Required]
        [MaxLength(200)]
        [Column("kurs_name")]
        public string KursName
        {
            get => _kursName;
            set => SetProperty(ref _kursName, value);
        }

        [Column("beschreibung")]
        public string Beschreibung
        {
            get => _beschreibung;
            set => SetProperty(ref _beschreibung, value);
        }

        [Required]
        [Column("preis", TypeName = "decimal(8,2)")]
        public decimal Preis
        {
            get => _preis;
            set => SetProperty(ref _preis, value);
        }

        [Column("max_teilnehmer")]
        public int MaxTeilnehmer
        {
            get => _maxTeilnehmer;
            set => SetProperty(ref _maxTeilnehmer, value);
        }

        [Required]
        [Column("start_datum")]
        public DateTime StartDatum
        {
            get => _startDatum;
            set => SetProperty(ref _startDatum, value);
        }

        [Column("end_datum")]
        public DateTime? EndDatum
        {
            get => _endDatum;
            set => SetProperty(ref _endDatum, value);
        }

        [MaxLength(20)]
        [Column("wochentag")]
        public string Wochentag
        {
            get => _wochentag;
            set => SetProperty(ref _wochentag, value);
        }

        [Column("uhrzeit")]
        public TimeSpan? Uhrzeit
        {
            get => _uhrzeit;
            set => SetProperty(ref _uhrzeit, value);
        }

        [Column("anzahl_termine")]
        public int AnzahlTermine
        {
            get => _anzahlTermine;
            set => SetProperty(ref _anzahlTermine, value);
        }

        [Column("lehrer_id")]
        public int? LehrerId
        {
            get => _lehrerId;
            set => SetProperty(ref _lehrerId, value);
        }

        [Column("ist_workshop")]
        public bool IstWorkshop
        {
            get => _istWorkshop;
            set => SetProperty(ref _istWorkshop, value);
        }

        // Navigation Properties
        [ForeignKey("LehrerId")]
        public Team Lehrer
        {
            get => _lehrer;
            set => SetProperty(ref _lehrer, value);
        }

        public List<Buchung> Buchungen
        {
            get => _buchungen ??= new List<Buchung>();
            set => SetProperty(ref _buchungen, value);
        }

        // Computed Properties für UI
        [NotMapped]
        public string KursTyp => IstWorkshop ? "Workshop" : "Kurs";

        [NotMapped]
        public int AnzahlTeilnehmer => Buchungen?.Count ?? 0;

        [NotMapped]
        public int VerfügbarePlätze => MaxTeilnehmer - AnzahlTeilnehmer;

        [NotMapped]
        public string ZeitAnzeige => Uhrzeit != null
            ? $"{Wochentag} {Uhrzeit:hh\\:mm}"
            : Wochentag ?? "Termin nach Vereinbarung";

        [NotMapped]
        public string DisplayText => $"{KursName} ({ZeitAnzeige}) - {Preis:C}";

        [NotMapped]
        public string DauerAnzeige => IstWorkshop
            ? $"{AnzahlTermine} Termin{(AnzahlTermine > 1 ? "e" : "")}"
            : $"{AnzahlTermine} Wochen";

        // Konstruktor
        public Kurs()
        {
            AnzahlTermine = 10; // Standard: 10 Termine
            MaxTeilnehmer = 20;
            StartDatum = DateTime.Today;
        }
    }
}