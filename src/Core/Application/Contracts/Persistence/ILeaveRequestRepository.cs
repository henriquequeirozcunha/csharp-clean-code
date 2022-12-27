
using Application.DTOs.LeaveRequests;
using Domain.Entities;

namespace Application.Contracts.Persistence
{
    public interface ILeaveRequestRepository : IGenericRepository<LeaveRequest>
    {
        Task<List<LeaveRequestDto>> GetLeaveRequestsWithDetails();
        Task<LeaveRequestDto> GetLeaveRequestWithDetails(int id);
    }
}