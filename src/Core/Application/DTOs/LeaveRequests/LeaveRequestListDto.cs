using Application.DTOs.LeaveTypes;
using Application.Models.Identity;

namespace Application.DTOs.LeaveRequests
{
    public class LeaveRequestListDto : BaseDto
    {
        public LeaveTypeDto LeaveType { get; set; }
        public DateTime DataRequested { get; set; }
        public bool? Approved { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Employee Employee { get; set; }
        public string RequestingEmployeeId { get; set; }
    }
}