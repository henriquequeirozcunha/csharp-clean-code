using Application.Contracts.Persistence;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories
{
    public class LeaveTypeRepository : GenericRepository<LeaveType>, ILeaveTypeRepository
    {
        private readonly LeaveManagementDbContext _dbContext;
        public LeaveTypeRepository(LeaveManagementDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}