using Microsoft.EntityFrameworkCore;

namespace ItaasSolution.Api.Infraestructure.DataAccess
{
    public class ItaasSolutionDbContext : DbContext
    {
        public ItaasSolutionDbContext(DbContextOptions options) : base(options) { }
        public DbSet<ItaasSolution.Api.Domain.Entities.Log> Log { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ItaasSolution.Api.Domain.Entities.Log>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(e => e.HtttpMethod)
                      .HasColumnType("varchar(20)");

                entity.Property(e => e.UriPath)
                      .HasColumnType("varchar(50)");

                entity.Property(e => e.TimeTaken)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.CacheStatus)
                      .HasColumnType("varchar(20)");
            });
        }
    }
}
