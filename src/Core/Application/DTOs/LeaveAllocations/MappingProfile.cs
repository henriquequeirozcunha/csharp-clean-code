using Application.DTOs.LeaveTypes;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOs.LeaveAllocationss
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LeaveType, LeaveTypeDto>().ReverseMap();
            CreateMap<LeaveType, CreateLeaveTypeDto>().ReverseMap();
            CreateMap<LeaveType, UpdateLeaveTypeDto>().ReverseMap();
        }
    }
}