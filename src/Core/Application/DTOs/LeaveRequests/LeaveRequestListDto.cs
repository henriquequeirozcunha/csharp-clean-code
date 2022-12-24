using Application.DTOs.LeaveTypes;

namespace Application.DTOs.LeaveRequests
{
    public class LeaveRequestListDto : BaseDto
    {
        public LeaveTypeDto LeaveType { get; set; }
        public DateTime DataRequested { get; set; }
        public bool? Approved { get; set; }
    }
}