using TSV.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSV.Models.Business
{
    [Table("buchung")]
    public class Buchung : ModelBase
    {
        private int _id;
        private int _kundeId;
        private int _kursId;
        private DateTime _buchungsdatum;
        private decimal? _bezahltBetrag;
        private DateTime? _bezahltAm;
        private string _notizen;
        private bool _storniert;
        private DateTime? _storniertAm;

        // Navigation Properties
        private Kunde _kunde;
        private Kurs _kurs;

        [Key]
        [Column("id")]
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        [Required]
        [Column("kunde_id")]
        public int KundeId
        {
            get => _kundeId;
            set => SetProperty(ref _kundeId, value);
        }

        [Required]
        [Column("kurs_id")]
        public int KursId
        {
            get => _kursId;
            set => SetProperty(ref _kursId, value);
        }

        [Required]
        [Column("buchungsdatum")]
        public DateTime Buchungsdatum
        {
            get => _buchungsdatum;
            set => SetProperty(ref _buchungsdatum, value);
        }

        [Column("bezahlt_betrag", TypeName = "decimal(8,2)")]
        public decimal? BezahltBetrag
        {
            get => _bezahltBetrag;
            set => SetProperty(ref _bezahltBetrag, value);
        }

        [Column("bezahlt_am")]
        public DateTime? BezahltAm
        {
            get => _bezahltAm;
            set => SetProperty(ref _bezahltAm, value);
        }

        [Column("notizen")]
        public string Notizen
        {
            get => _notizen;
            set => SetProperty(ref _notizen, value);
        }

        [Column("storniert")]
        public bool Storniert
        {
            get => _storniert;
            set => SetProperty(ref _storniert, value);
        }

        [Column("storniert_am")]
        public DateTime? StorniertAm
        {
            get => _storniertAm;
            set => SetProperty(ref _storniertAm, value);
        }

        // Navigation Properties
        [ForeignKey("KundeId")]
        public Kunde Kunde
        {
            get => _kunde;
            set => SetProperty(ref _kunde, value);
        }

        [ForeignKey("KursId")]
        public Kurs Kurs
        {
            get => _kurs;
            set => SetProperty(ref _kurs, value);
        }

        // Computed Properties für UI
        [NotMapped]
        public bool IstBezahlt => BezahltBetrag.HasValue && BezahltAm.HasValue;

        [NotMapped]
        public decimal OffenerBetrag => (Kurs?.Preis ?? 0) - (BezahltBetrag ?? 0);

        [NotMapped]
        public string BezahlStatus => Storniert ? "Storniert" :
                                     IstBezahlt ? "Bezahlt" :
                                     "Offen";

        [NotMapped]
        public string DisplayText => $"{Kunde?.VollName} → {Kurs?.KursName} ({BezahlStatus})";

        // Konstruktor
        public Buchung()
        {
            Buchungsdatum = DateTime.Now;
        }
    }
}