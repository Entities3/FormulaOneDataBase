namespace Formula1.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Race
    {
        public int Id { get; set; }

        [Required]
        public string Position { get; set; }

        public int GrandPrixId { get; set; }

        public virtual GrandPrix GrandPrix { get; set; }

        public int SeasonId { get; set; }

        public virtual Season Season { get; set; }

        public int DriverId { get; set; }

        public virtual Driver Driver { get; set; }

        public int ConstructorId { get; set; }

        public virtual Constructor Constructor { get; set; }

        [Required]
        [Range(0, 25, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Score { get; set; }
    }
}
