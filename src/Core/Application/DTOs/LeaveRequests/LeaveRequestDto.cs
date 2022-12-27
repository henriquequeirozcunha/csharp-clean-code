using Application.DTOs.LeaveTypes;

namespace Application.DTOs.LeaveRequests
{
    public class LeaveRequestDto : BaseDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveTypeDto LeaveType { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime? DataRequested { get; set; }
        public string RequestComments { get; set; }
        public DateTime DataActioned { get; set; }
        public bool? Approved { get; set; }
        public bool Cancelled { get; set; }
    }
}