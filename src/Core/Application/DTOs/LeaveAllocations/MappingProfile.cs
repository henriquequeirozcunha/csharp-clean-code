using Application.DTOs.LeaveTypes;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOs.LeaveAllocations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LeaveType, LeaveTypeDto>().ReverseMap();
        }
    }
}