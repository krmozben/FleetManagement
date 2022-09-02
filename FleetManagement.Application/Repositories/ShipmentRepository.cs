using FleetManagement.Domain.ShipmentAggregate;
using FleetManagement.Infrastructure;
using FleetManagement.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FleetManagement.Application.Repositories
{
    public class ShipmentRepository : IRepository<Shipment>
    {
        private readonly FleetDbContext _context;

        public ShipmentRepository(FleetDbContext context) => _context = context;

        public async Task AddRangeAsync(List<Shipment> entity)
        {
            await _context.Shipments.AddRangeAsync(entity);
            await SaveChangesAsync();
        }

        public Task BulkUpdateAsync(List<Shipment> entities)
        {
            entities.ForEach(x => _context.Entry(x).State = EntityState.Modified);

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Shipment>> GetAsync(Expression<Func<Shipment, bool>> predicate)
        {
            return await _context.Set<Shipment>().Where(predicate).Include(i => i.PackageSack).ThenInclude(t => t.Sack).Include(i => i.DeliveryPoint).ToListAsync();
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
        public async Task MigrateDatabaseAsync() => await _context.Database.MigrateAsync();
    }

}
