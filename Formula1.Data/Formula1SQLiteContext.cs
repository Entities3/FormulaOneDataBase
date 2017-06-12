namespace Formula1.Data
{
    using Models;
    using System.Data.Entity;

    public class Formula1SQLiteContext : DbContext
    {
        public Formula1SQLiteContext()
            : base("name=Formula1SQLite")
        {
        }

        public virtual IDbSet<Country> Countries { get; set; }

        public virtual IDbSet<GrandPrix> GrandPrixes { get; set; }
    }
}