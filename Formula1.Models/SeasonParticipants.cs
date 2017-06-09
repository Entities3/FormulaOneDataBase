namespace Formula1.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SeasonParticipants
    {
        public int Id { get; set; }

        [Required]
        public virtual Season Season { get; set; }

        [Range(1, 99, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int DriverPermanentNumber { get; set; }

        [Required]
        public virtual Driver Driver { get; set; }

        [Required]
        public virtual Constructor Constructor { get; set; }
    }
}
