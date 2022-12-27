namespace Application.DTOs.LeaveTypes
{
    public class UpdateLeaveTypeDto : BaseDto
    {
        public string Name { get; set; }
        public int DefaultDays { get; set; }
    }
}