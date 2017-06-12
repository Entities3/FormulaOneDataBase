namespace Formula1.Data
{
    using Models;
    using SQLiteMigrations;
    using System.Data.Entity;

    public class Formula1SQLiteContext : DbContext
    {
        public Formula1SQLiteContext()
            : base("Formula1SQLite")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Formula1SQLiteContext, SQLiteConfiguration>(true));
        }
        //public virtual IDbSet<Circuit> Circuits { get; set; }

        public virtual IDbSet<Country> Countries { get; set; }
    }
}