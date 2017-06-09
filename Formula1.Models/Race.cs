namespace Formula1.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Race
    {
        public int Id { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        public virtual Circuit Circuit { get; set; }

        [Required]
        public virtual Season Season { get; set; }

        [Required]
        public virtual Driver Driver { get; set; }

        [Required]
        public virtual Constructor Constructor { get; set; }

        [Required]
        [Range(0, 25, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Score { get; set; }
    }
}
