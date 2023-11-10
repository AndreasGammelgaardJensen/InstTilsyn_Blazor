using Microsoft.EntityFrameworkCore;
using ModelsLib.DatabaseModels;

namespace TilsynsRapportApi.Database
{
    public class ReportDBContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ReportDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=localhost,1433;Initial Catalog=Institution_kk_reportKriteriaDB;User ID=SA;Password=And12345;TrustServerCertificate=True;");
            //optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SqlServer"));

        }

        public IQueryable<TDatabaseModel> Get<TDatabaseModel>() where TDatabaseModel : class
        {
            return base.Set<TDatabaseModel>().AsNoTracking().AsQueryable();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InstitutionReportCriterieaDatabasemodel>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<CategoriClass>()
                .HasKey(a => a.Id);

        }

        public DbSet<InstitutionReportCriterieaDatabasemodel> InstitutionReportCriterieaDatabasemodel => Set<InstitutionReportCriterieaDatabasemodel>();
    }
}
