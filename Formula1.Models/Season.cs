namespace Formula1.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Season
    {
        //    private ICollection<Circuit> circuits;
        //   private ICollection<Constructor> constructors;
        //    private ICollection<Driver> drivers;
        private ICollection<Race> races;

        public Season()
        {
            this.races = new HashSet<Race>();
            //       this.circuits = new HashSet<Circuit>();
            //     this.constructors = new HashSet<Constructor>();
            //     this.drivers = new HashSet<Driver>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(4)]
        [Index(IsUnique = true)]
        public string Year { get; set; }

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

        //  public virtual ICollection<Circuit> Circuits
        //  {
        //      get
        //      {
        //          return this.circuits;
        //      }
        //      set
        //      {
        //          this.circuits = value;
        //      }
        //  }

        //    public virtual ICollection<Constructor> Constructors
        //    {
        //        get
        //        {
        //            return this.constructors;
        //        }
        //        set
        //        {
        //            this.constructors = value;
        //        }
        //    }
        //
        //    public virtual ICollection<Driver> Drivers
        //    {
        //        get
        //        {
        //            return this.drivers;
        //        }
        //        set
        //        {
        //            this.drivers = value;
        //        }
        //    }

        [Column(TypeName = "ntext")]
        public string ReviewUrl { get; set; }
    }
}
