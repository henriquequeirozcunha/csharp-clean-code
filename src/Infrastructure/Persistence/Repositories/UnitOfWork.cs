using Application.Contracts.Persistence;

namespace Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LeaveManagementDbContext _context;

        private ILeaveAllocationRepository _leaveAllocationRepository;
        private ILeaveRequestRepository _leaveRequestRepository;
        private ILeaveTypeRepository _leaveTypeRepository;

        public UnitOfWork(LeaveManagementDbContext context)
        {
            _context = context;
        }

        public ILeaveAllocationRepository leaveAllocationRepository => _leaveAllocationRepository ?? new LeaveAllocationRepository(_context);

        public ILeaveRequestRepository leaveRequestRepository => _leaveRequestRepository ?? new LeaveRequestRepository(_context);

        public ILeaveTypeRepository leaveTypeRepository => _leaveTypeRepository ?? new LeaveTypeRepository(_context);

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}