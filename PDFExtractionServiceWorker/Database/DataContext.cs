using Microsoft.EntityFrameworkCore;
using ModelsLib.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PDFExtractionServiceWorker.Database
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=localhost.,1433;Database=InstitutionTestDb;Trusted_Connection=True;");
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SqlServer"));

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
