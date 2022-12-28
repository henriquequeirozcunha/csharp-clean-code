using Application.Contracts.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>, ILeaveAllocationRepository
    {
        private readonly LeaveManagementDbContext _dbContext;
        public LeaveAllocationRepository(LeaveManagementDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails()
        {
            return await _dbContext.LeaveAllocations.Include(r => r.LeaveType).ToListAsync();
        }

        public async Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id)
        {
            return await _dbContext.LeaveAllocations.Include(r => r.LeaveType).FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}