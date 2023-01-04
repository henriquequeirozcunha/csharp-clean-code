using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public abstract class AuditableDbContext : DbContext
    {
        public AuditableDbContext(DbContextOptions options) : base(options)
        {

        }

        public virtual async Task<int> SaveChangesAsync(string username = "SYSTEM")
        {
            var addedOrModifiedEntries = base.ChangeTracker.Entries<BaseDomainEntity>().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
            foreach (var entry in addedOrModifiedEntries)
            {
                entry.Entity.LastModifiedDate = DateTime.Now;
                entry.Entity.LastModifiedBy = username;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.DataCreated = DateTime.Now;
                    entry.Entity.CreatedBy = username;
                }
            }

            var result = await base.SaveChangesAsync();

            return result;
        }
    }
}