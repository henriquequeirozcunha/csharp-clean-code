using Application.Contants;
using Application.Contracts.Persistence;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LeaveManagementDbContext _context;
        private readonly IHttpContextAccessor _httpContentAccessor;

        private ILeaveAllocationRepository _leaveAllocationRepository;
        private ILeaveRequestRepository _leaveRequestRepository;
        private ILeaveTypeRepository _leaveTypeRepository;

        public UnitOfWork(LeaveManagementDbContext context, IHttpContextAccessor httpContentAccessor)
        {
            _httpContentAccessor = httpContentAccessor;
            _context = context;
        }

        public ILeaveAllocationRepository leaveAllocationRepository => _leaveAllocationRepository ?? new LeaveAllocationRepository(_context);

        public ILeaveRequestRepository leaveRequestRepository => _leaveRequestRepository ?? new LeaveRequestRepository(_context);

        public ILeaveTypeRepository leaveTypeRepository => _leaveTypeRepository ?? new LeaveTypeRepository(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            var username = _httpContentAccessor.HttpContext.User.FindFirst(CustomClaimTypes.Uid)?.Value;

            await _context.SaveChangesAsync(username);
        }
    }
}