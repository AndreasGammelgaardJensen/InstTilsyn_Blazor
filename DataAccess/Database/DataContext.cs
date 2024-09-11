using Microsoft.EntityFrameworkCore;
using ModelsLib.DatabaseModels;
using System.Reflection.Emit;

namespace DataAccess.Database
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

			/*
             * dotnet ef migrations add InitialCreate
             * dotnet ef database update

             */
			//used when running EF updates
			//optionsBuilder.UseSqlServer(@"Data Source=localhost,1433;Initial Catalog=TestDB;User ID=SA;Password=And12345;TrustServerCertificate=True;");
			//optionsBuilder.UseSqlServer(@"Server=localhost.,1433;Database=InstitutionTestDb;Trusted_Connection=True;");
			//optionsBuilder.UseSqlServer(@"Data Source=InstitutionDB,1433;Initial Catalog=InstitutionDB;User ID=SA;Password=And12345;TrustServerCertificate=True;", options => options.EnableRetryOnFailure());
			//optionsBuilder.UseSqlServer(@"Data Source=localhost,1433;Initial Catalog=Test2;User ID=SA;Password=And12345;TrustServerCertificate=True;");
		}

		public IQueryable<TDatabaseModel> Get<TDatabaseModel>() where TDatabaseModel : class
        {
            return base.Set<TDatabaseModel>().AsNoTracking().AsQueryable();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InstitutionFrontPageModelDatabasemodel>()
                .HasKey(a => a.Id);

			modelBuilder.Entity<InstitutionTilsynsRapportDatabasemodel>()
				.HasKey(a => a.Id);

			modelBuilder.Entity<AddressDatabasemodel>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<PladserDatabasemodel>()
               .HasKey(a => a.Id);

            modelBuilder.Entity<InstitutionReportCriterieaDatabasemodel>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<CategoriClass>()
                .HasKey(a => a.Id);

			modelBuilder.Entity<ContactDatabasemodel>()
				.HasKey(a => a.Id);
		}

        public DbSet<InstitutionFrontPageModelDatabasemodel> InstitutionFrontPageModel => Set<InstitutionFrontPageModelDatabasemodel>();
        public DbSet<InstitutionReportCriterieaDatabasemodel> InstitutionReportCriterieaDatabasemodel => Set<InstitutionReportCriterieaDatabasemodel>();
		public DbSet<AddressDatabasemodel> AddressDatabasemodel => Set<AddressDatabasemodel>();
        public DbSet<InstKoordinatesDatabasemodel> InstKoordinatesDatabasemodel => Set<InstKoordinatesDatabasemodel>();
		public DbSet<InstitutionTilsynsRapportDatabasemodel> InstitutionTilsynsRapportDatabasemodel => Set<InstitutionTilsynsRapportDatabasemodel>();

		public DbSet<ContactDatabasemodel> ContactDatabasemodel => Set<ContactDatabasemodel>();



	}
}
