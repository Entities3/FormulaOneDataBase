namespace Formula1.Data
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Models;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Formula1Context : DbContext
    {
        public Formula1Context()
           : base("Formula1DataBase")
        {
        }

        public virtual IDbSet<Driver> Drivers { get; set; }

        public virtual IDbSet<Nationality> Nationaties { get; set; }

        //public virtual IDbSet<Circuit> Circuits { get; set; }

        //public virtual IDbSet<Country> Countries { get; set; }

        public virtual IDbSet<Constructor> Constructors { get; set; }

        public virtual IDbSet<Race> Races { get; set; }

        public virtual IDbSet<Season> Seasons { get; set; }

        public virtual IDbSet<SeasonParticipants> SeasonsParticipants { get; set; }

        public virtual IDbSet<GrandPrix> GrandPrixes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {                                   
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Race>()
                .HasRequired(r => r.Season).WithMany(r => r.Races).HasForeignKey(r => r.SeasonId);
            modelBuilder.Entity<Race>()
                .HasRequired(r => r.GrandPrix).WithMany(r => r.Races).HasForeignKey(r => r.GrandPrixId);
            modelBuilder.Entity<Race>()
               .HasRequired(r => r.Driver).WithMany(r => r.Races).HasForeignKey(r => r.DriverId);
            modelBuilder.Entity<Race>()
               .HasRequired(r => r.Constructor).WithMany(r => r.Races).HasForeignKey(r => r.ConstructorId);

            modelBuilder.Entity<Race>()
                .Property(r => r.SeasonId)
                .IsRequired()
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_Race_Driver", 1) { IsUnique = true }));

            modelBuilder.Entity<Race>()
               .Property(r => r.GrandPrixId)
               .IsRequired()
               .HasColumnAnnotation(
                   IndexAnnotation.AnnotationName,
                   new IndexAnnotation(
                       new IndexAttribute("IX_Race_Driver", 2) { IsUnique = true }));

            modelBuilder.Entity<Race>()
               .Property(r => r.DriverId)
               .IsRequired()
               .HasColumnAnnotation(
                   IndexAnnotation.AnnotationName,
                   new IndexAnnotation(
                       new IndexAttribute("IX_Race_Driver", 3) { IsUnique = true })); 

            base.OnModelCreating(modelBuilder);
        }
    }
}
