using TSV.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSV.Models.Business
{
    [Table("team")]
    public class Team : ModelBase
    {
        private int _id;
        private string _vorname;
        private string _nachname;
        private string _email;
        private string _telefon;
        private string _position;
        private decimal? _gehalt;
        private DateTime _eingestellt;

        // Navigation Properties
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

        [MaxLength(100)]
        [Column("position")]
        public string Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        [Column("gehalt", TypeName = "decimal(10,2)")]
        public decimal? Gehalt
        {
            get => _gehalt;
            set => SetProperty(ref _gehalt, value);
        }

        [Column("eingestellt")]
        public DateTime Eingestellt
        {
            get => _eingestellt;
            set => SetProperty(ref _eingestellt, value);
        }

        // Navigation Properties
        public List<Kurs> Kurse
        {
            get => _kurse ??= new List<Kurs>();
            set => SetProperty(ref _kurse, value);
        }

        // Computed Properties für UI
        [NotMapped]
        public string VollName => $"{Vorname} {Nachname}";

        [NotMapped]
        public string DisplayText => $"{VollName} ({Position})";

        // Konstruktor
        public Team()
        {
            Eingestellt = DateTime.Now;
        }
    }
}