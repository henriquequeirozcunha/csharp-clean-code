
using Application.DTOs.LeaveAllocations;
using Domain.Entities;

namespace Application.Contracts.Persistence
{
    public interface ILeaveAllocationRepository : IGenericRepository<LeaveAllocation>
    {
        Task<List<LeaveAllocationDto>> GetLeaveAllocationsWithDetails();
        Task<LeaveAllocationDto> GetLeaveAllocationWithDetails(int id);
    }
}