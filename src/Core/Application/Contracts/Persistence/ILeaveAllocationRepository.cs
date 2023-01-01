
using Application.DTOs.LeaveAllocations;
using Domain.Entities;

namespace Application.Contracts.Persistence
{
    public interface ILeaveAllocationRepository : IGenericRepository<LeaveAllocation>
    {
        Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails();
        Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id);

        Task<bool> AllocationExists(string userId, int leaveTypeId, int period);

        Task AddAllocations(List<LeaveAllocation> allocations);
    }
}