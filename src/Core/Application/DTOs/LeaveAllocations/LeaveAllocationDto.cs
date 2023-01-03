using Application.DTOs.LeaveTypes;
using Application.Models.Identity;

namespace Application.DTOs.LeaveAllocations
{
    public class LeaveAllocationDto : BaseDto, ILeaveAllocationDto
    {
        public int NumberOfDays { get; set; }
        public LeaveTypeDto LeaveType { get; set; }
        public int LeaveTypeId { get; set; }
        public int Period { get; set; }
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}