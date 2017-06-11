namespace Formula1.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Driver
    {
        private ICollection<Season> seasons;
        private ICollection<Race> races;

        public Driver()
        {
            this.seasons = new HashSet<Season>();
            this.races = new HashSet<Race>();
        }

        public int Id { get; set; }

        [Required()]
        [MinLength(2)]
        [MaxLength(50)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [Required]
        public virtual Nationality Nationality { get; set; }

        public virtual ICollection<Season> Seasons
        {
            get
            {
                return this.seasons;
            }
            set
            {
                this.seasons = value;
            }
        }

        public virtual ICollection<Race> Races
        {
            get
            {
                return this.races;
            }
            set
            {
                this.races = value;
            }
        }

        [Column(TypeName = "ntext")]
        public string InformationUrl { get; set; }
    }
}
