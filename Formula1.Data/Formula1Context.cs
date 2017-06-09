namespace Formula1.Data
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Models;  

    public class Formula1Context : DbContext
    {
        public Formula1Context()
           : base("Formula1DataBase")
        {
        }

        public virtual IDbSet<Driver> Drivers { get; set; }

        public virtual IDbSet<Nationality> Nationaties { get; set; }

        public virtual IDbSet<Circuit> Circuits { get; set; }

        public virtual IDbSet<Country> Countries { get; set; }

        public virtual IDbSet<Constructor> Constructors { get; set; }

        public virtual IDbSet<Race> Races { get; set; }

        public virtual IDbSet<Season> Seasons { get; set; }

        public virtual IDbSet<SeasonParticipants> SeasonsParticipants { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //          modelBuilder.Entity<Driver>()
            //              .HasMany(d=>d.Seasons)
            //             .WithMany(s=>s.Drivers)
            //              .Map(m =>
            //              {
            //                 m.ToTable("SeasonParticipants");
            //                 m.MapLeftKey("Season_Id");
            //                 m.MapRightKey("Driver_Id");
            //             });

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
