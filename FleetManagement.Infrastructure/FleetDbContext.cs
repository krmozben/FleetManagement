using FleetManagement.Domain.ShipmentAggregate;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Infrastructure
{
    public class FleetDbContext : DbContext
    {
        public FleetDbContext(DbContextOptions<FleetDbContext> options) : base(options)
        {
        }

        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<DeliveryPoint> DeliveryPoints { get; set; }
        public DbSet<PackageSack> PackageSacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shipment>().OwnsOne(o => o.Desi, on => { on.Property(p => p.Value).HasColumnName("Desi"); });

            modelBuilder.Entity<Shipment>().HasOne(o => o.PackageSack).WithOne(x=>x.Package);

            base.OnModelCreating(modelBuilder);
        }
    }
}
