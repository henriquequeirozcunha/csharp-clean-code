using Application.Contants;
using Application.Contracts.Identity;
using Application.Contracts.Persistence;
using Application.DTOs.LeaveAllocations;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.LeaveAllocations
{
    public class GetLeaveAllocationList
    {
        public class Query : IRequest<List<LeaveAllocationDto>>
        {
            public bool IsLoggedInUser { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<LeaveAllocationDto>>
        {
            private readonly ILeaveAllocationRepository _leaveAllocationRepository;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _httpContext;
            private readonly IUserService _userService;
            public Handler(ILeaveAllocationRepository leaveAllocationRepository,
                IMapper mapper,
                IHttpContextAccessor httpContext,
                IUserService userService)
            {
                _httpContext = httpContext;
                _mapper = mapper;
                _leaveAllocationRepository = leaveAllocationRepository;
                _userService = userService;
            }

            public async Task<List<LeaveAllocationDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var leaveAllocations = new List<LeaveAllocation>();
                var allocations = new List<LeaveAllocationDto>();

                if (request.IsLoggedInUser)
                {
                    var userId = _httpContext.HttpContext.User.FindFirst(q => q.Type == CustomClaimTypes.Uid)?.Value;

                    leaveAllocations = await _leaveAllocationRepository.GetLeaveAllocationsWithDetails(userId);

                    var employee = await _userService.GetEmployee(userId);

                    allocations = _mapper.Map<List<LeaveAllocationDto>>(leaveAllocations);

                    foreach (var alloc in allocations)
                    {
                       alloc.Employee  = employee;
                    }

                    return allocations;
                }

                leaveAllocations = await _leaveAllocationRepository.GetLeaveAllocationsWithDetails();
                allocations = _mapper.Map<List<LeaveAllocationDto>>(leaveAllocations);

                foreach (var alloc in allocations)
                {
                    alloc.Employee = await _userService.GetEmployee(alloc.EmployeeId);
                }

                return allocations;
            }
        }
    }
}