
using Application.DTOs.LeaveRequests;
using Domain.Entities;

namespace Application.Contracts.Persistence
{
    public interface ILeaveRequestRepository : IGenericRepository<LeaveRequest>
    {
        Task<List<LeaveRequest>> GetLeaveRequestsWithDetails();
        Task<LeaveRequest> GetLeaveRequestWithDetails(int id);
        Task ChangeApprovalStatus(LeaveRequest leaveRequest, bool? ApprovalStatus);
    }
}