using AutoMapper;
using Domain.Entities;

namespace Application.DTOs.LeaveTypes
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LeaveType, LeaveTypeDto>().ReverseMap();
        }
    }
}