using AutoMapper;
using Domain.Entities;

namespace Application.DTOs.LeaveRequests
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LeaveRequest, LeaveRequestDto>().ReverseMap();
            CreateMap<LeaveRequest, LeaveRequestListDto>().ReverseMap();
        }
    }
}