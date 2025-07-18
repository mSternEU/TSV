﻿using TSV.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSV.Models.Business
{
    [Table("kunde")]
    public partial class Kunde : ModelBase
    {
        private int _id;
        private string _vorname;
        private string _nachname;
        private string _strasse;
        private int _plz;
        private string _ort;
        private string _telefon;
        private string _mail;
        private DateTime? _geburtsdatum;
        private int? _geschlechtId;
        private int? _zahlweiseId;
        private int? _koerpergroesse;
        private string _notes;
        private DateTime _erstelltAm;
        private bool _istAktiv;
        private DateTime? _geloeschtAm;
        private int? _geloeschtVon;
        private int? _kundeRolleId;

        // Navigation Properties
        private Geschlecht _geschlecht;
        private Zahlweise _zahlweise;
        private List<Buchung> _buchungen;
        private KundeRolle _kundeRolle;

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

        [Required]
        [MaxLength(200)]
        [Column("strasse")]
        public string Strasse
        {
            get => _strasse;
            set => SetProperty(ref _strasse, value);
        }

        [Required]
        [Column("plz")]
        public int Plz
        {
            get => _plz;
            set => SetProperty(ref _plz, value);
        }

        [Required]
        [MaxLength(100)]
        [Column("ort")]
        public string Ort
        {
            get => _ort;
            set => SetProperty(ref _ort, value);
        }

        [Required]
        [MaxLength(50)]
        [Column("telefon")]
        public string Telefon
        {
            get => _telefon;
            set => SetProperty(ref _telefon, value);
        }

        [Required]
        [MaxLength(200)]
        [Column("mail")]
        public string Mail
        {
            get => _mail;
            set => SetProperty(ref _mail, value);
        }

        [Column("geburtsdatum")]
        public DateTime? Geburtsdatum
        {
            get => _geburtsdatum;
            set => SetProperty(ref _geburtsdatum, value);
        }

        [Column("geschlecht_id")]
        public int? GeschlechtId
        {
            get => _geschlechtId;
            set => SetProperty(ref _geschlechtId, value);
        }

        [Column("zahlweise_id")]
        public int? ZahlweiseId
        {
            get => _zahlweiseId;
            set => SetProperty(ref _zahlweiseId, value);
        }

        [Column("kunde_rolle_id")]
        public int? KundeRolleId
        {
            get => _kundeRolleId;
            set => SetProperty(ref _kundeRolleId, value);
        }

        [Column("koerpergroesse")]
        public int? Koerpergroesse
        {
            get => _koerpergroesse;
            set => SetProperty(ref _koerpergroesse, value);
        }

        [Column("notes")]
        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        [Column("erstellt_am")]
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
        [ForeignKey("GeschlechtId")]
        public Geschlecht Geschlecht
        {
            get => _geschlecht;
            set => SetProperty(ref _geschlecht, value);
        }

        [ForeignKey("ZahlweiseId")]
        public Zahlweise Zahlweise
        {
            get => _zahlweise;
            set => SetProperty(ref _zahlweise, value);
        }

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
        public string DisplayName => $"{VollName} ({Mail})";

        [NotMapped]
        public int Alter => Geburtsdatum?.Date != null
            ? DateTime.Today.Year - Geburtsdatum.Value.Year -
              (DateTime.Today.DayOfYear < Geburtsdatum.Value.DayOfYear ? 1 : 0)
            : 0;

        // Konstruktor
        public Kunde()
        {
            ErstelltAm = DateTime.Now;
            IstAktiv = true;
        }
    }
}