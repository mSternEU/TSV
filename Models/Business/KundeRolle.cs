using TSV.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSV.Models.Business
{
    [Table("kunde_rolle")]
    public class KundeRolle : ModelBase
    {
        private int _id;
        private string _rolleName;

        // Navigation Properties
        private List<Kunde> _kunden;

        [Key]
        [Column("id")]
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        [Required]
        [MaxLength(100)]
        [Column("rolle_name")]
        public string RolleName
        {
            get => _rolleName;
            set => SetProperty(ref _rolleName, value);
        }

        // Navigation Properties
        public List<Kunde> Kunden
        {
            get => _kunden ??= new List<Kunde>();
            set => SetProperty(ref _kunden, value);
        }
    }
}