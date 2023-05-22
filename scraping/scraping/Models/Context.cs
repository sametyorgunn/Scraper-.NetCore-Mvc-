using Microsoft.EntityFrameworkCore;

namespace scraping.Models
{
    public class Context:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=SAMET;Initial Catalog=scraping;Integrated Security=True");
        }
        public DbSet<Urun> Uruns { get; set; }
        public DbSet<Excel> excels { get; set; }
        public DbSet<Kullanici> kullanicis { get; set; }

    }
}
