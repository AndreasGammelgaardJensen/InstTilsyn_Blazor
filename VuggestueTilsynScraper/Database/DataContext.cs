using Microsoft.EntityFrameworkCore;
using ModelsLib.DatabaseModels;

namespace VuggestueTilsynScraper.Database
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=localhost.,1433;Database=InstitutionTestDb;Trusted_Connection=True;");
            //optionsBuilder.UseSqlServer(@"Data Source=InstitutionDB,1433;Initial Catalog=InstitutionDB;User ID=SA;Password=And12345;TrustServerCertificate=True;");
            //optionsBuilder.UseSqlServer(@"Data Source=localhost,1433;Initial Catalog=InstitutionDB;User ID=SA;Password=And12345;TrustServerCertificate=True;");

        }

        public IQueryable<TDatabaseModel> Get<TDatabaseModel>() where TDatabaseModel : class
        {
            return base.Set<TDatabaseModel>().AsNoTracking().AsQueryable();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InstitutionFrontPageModelDatabasemodel>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<AddressDatabasemodel>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<PladserDatabasemodel>()
               .HasKey(a => a.Id);

        }
        
        public DbSet<InstitutionFrontPageModelDatabasemodel> InstitutionFrontPageModel => Set<InstitutionFrontPageModelDatabasemodel>();
    }
}

