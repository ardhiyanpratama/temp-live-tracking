using BackendService.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<VehicleChamber> VehicleChambers { get; set; } = null!;
        public virtual DbSet<AdditionalInformation> AdditionalInformation { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            base.OnModelCreating(modelBuilder);

        }

    }
}
