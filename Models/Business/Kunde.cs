using TSV.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSV.Models.Business
{
    [Table("kunde")]
    public class Kunde : ModelBase
    {
        private int _id;
        private string _vorname;
        private string _nachname;
        private string _email;
        private string _telefon;
        private DateTime? _geburtsdatum;
        private string _adresse;
        private string _notizen;
        private DateTime _erstelltAm;
        private int? _kundeRolleId;

        // Navigation Properties
        private KundeRolle _kundeRolle;
        private List<Buchung> _buchungen;

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

        [MaxLength(200)]
        [Column("email")]
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        [MaxLength(50)]
        [Column("telefon")]
        public string Telefon
        {
            get => _telefon;
            set => SetProperty(ref _telefon, value);
        }

        [Column("geburtsdatum")]
        public DateTime? Geburtsdatum
        {
            get => _geburtsdatum;
            set => SetProperty(ref _geburtsdatum, value);
        }

        [MaxLength(500)]
        [Column("adresse")]
        public string Adresse
        {
            get => _adresse;
            set => SetProperty(ref _adresse, value);
        }

        [Column("notizen")]
        public string Notizen
        {
            get => _notizen;
            set => SetProperty(ref _notizen, value);
        }

        [Column("erstellt_am")]
        public DateTime ErstelltAm
        {
            get => _erstelltAm;
            set => SetProperty(ref _erstelltAm, value);
        }

        [Column("kunde_rolle_id")]
        public int? KundeRolleId
        {
            get => _kundeRolleId;
            set => SetProperty(ref _kundeRolleId, value);
        }

        // Navigation Properties
        [ForeignKey("KundeRolleId")]
        public KundeRolle KundeRolle
        {
            get => _kundeRolle;
            set => SetProperty(ref _kundeRolle, value);
        }

        public List<Buchung> Buchungen
        {
            get => _buchungen ??= new List<Buchung>();
            set => SetProperty(ref _buchungen, value);
        }

        // Computed Properties für UI
        [NotMapped]
        public string VollName => $"{Vorname} {Nachname}";

        [NotMapped]
        public int Alter => Geburtsdatum?.Date != null
            ? DateTime.Today.Year - Geburtsdatum.Value.Year -
              (DateTime.Today.DayOfYear < Geburtsdatum.Value.DayOfYear ? 1 : 0)
            : 0;

        [NotMapped]
        public string DisplayText => $"{VollName} ({Email})";

        // Konstruktor
        public Kunde()
        {
            ErstelltAm = DateTime.Now;
        }
    }
}