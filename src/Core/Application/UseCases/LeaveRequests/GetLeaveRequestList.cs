using Application.Contants;
using Application.Contracts.Identity;
using Application.Contracts.Persistence;
using Application.DTOs.LeaveRequests;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.LeaveRequests
{
    public class GetLeaveRequestList
    {
        public class Query : IRequest<List<LeaveRequestListDto>>
        {
            public bool IsLoggedInUser { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<LeaveRequestListDto>>
        {
            private readonly ILeaveRequestRepository _leaveRequestRepository;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _httpContext;
            private readonly IUserService _userService;

            public Handler(ILeaveRequestRepository leaveRequestRepository,
                IMapper mapper,
                IHttpContextAccessor httpContext,
                IUserService userService)
            {
                _httpContext = httpContext;
                _mapper = mapper;
                _leaveRequestRepository = leaveRequestRepository;
                _userService = userService;
            }

            public async Task<List<LeaveRequestListDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var leaveRequests = new List<LeaveRequest>();
                var requests = new List<LeaveRequestListDto>();

                if (request.IsLoggedInUser)
                {
                    var userId = _httpContext.HttpContext.User.FindFirst(q => q.Type == CustomClaimTypes.Uid)?.Value;

                    leaveRequests = await _leaveRequestRepository.GetLeaveRequestsWithDetails(userId);

                    var employee = await _userService.GetEmployee(userId);

                    requests = _mapper.Map<List<LeaveRequestListDto>>(leaveRequests);

                    foreach (var req in requests)
                    {
                        req.Employee = employee;
                    }

                    return requests;
                }

                leaveRequests = await _leaveRequestRepository.GetLeaveRequestsWithDetails();
                requests = _mapper.Map<List<LeaveRequestListDto>>(leaveRequests);

                foreach (var req in requests)
                {
                    req.Employee = await _userService.GetEmployee(req.RequestingEmployeeId);
                }

                return requests;
            }
        }
    }
}