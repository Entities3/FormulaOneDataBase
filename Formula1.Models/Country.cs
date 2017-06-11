namespace Formula1.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Countries")]
    public class Country
    {
        public int Id { get; set; }

        [Required()]
        [MinLength(2)]
        [MaxLength(50)]
        [Index(IsUnique = true)]
        public string Name { get; set; }
    }
}
