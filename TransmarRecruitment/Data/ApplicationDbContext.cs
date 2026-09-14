using Microsoft.EntityFrameworkCore;
using TransmarRecruitment.Models;

namespace TransmarRecruitment.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<AssemblyLine> AssemblyLines { get; set; } = null!;
        public DbSet<Workstation> Workstations { get; set; } = null!;
        public DbSet<AssemblyLineWorkstation> AssemblyLineWorkstations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name);

            modelBuilder.Entity<AssemblyLineWorkstation>()
                .HasIndex(a => new { a.AssemblyLineId, a.Position });

            modelBuilder.Entity<AssemblyLineWorkstation>()
                .HasOne(a => a.AssemblyLine)
                .WithMany(al => al.Allocations)
                .HasForeignKey(a => a.AssemblyLineId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AssemblyLineWorkstation>()
                .HasOne(a => a.Workstation)
                .WithMany(w => w.Allocations)
                .HasForeignKey(a => a.WorkstationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
