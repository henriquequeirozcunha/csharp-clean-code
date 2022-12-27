namespace Application.DTOs.LeaveTypes
{
    public class UpdateLeaveTypeDto : BaseDto, ILeaveTypeDto
    {
        public string Name { get; set; }
        public int DefaultDays { get; set; }
    }
}