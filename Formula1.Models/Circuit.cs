namespace Formula1.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Circuit
    {
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [Required]
        public Country Country { get; set; }

        public string Locality { get; set; }

        [Column(TypeName = "ntext")]
        public string InformationUrl { get; set; }
    }
}
