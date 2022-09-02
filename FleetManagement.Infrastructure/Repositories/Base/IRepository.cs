using FleetManagement.DomainCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FleetManagement.Infrastructure.Repositories.Base
{
    public interface IRepository<T> where T : Entity, IAggregateRoot
    {
        public Task AddRangeAsync(List<T> entity);
        public Task BulkUpdateAsync(List<T> entities);   
        public Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate);
        public Task SaveChangesAsync();
        public Task MigrateDatabaseAsync();
    }
}
