using Application.Contracts.Persistence;
using Application.DTOs.LeaveRequests;
using AutoMapper;
using MediatR;

namespace Application.UseCases.LeaveRequest
{
    public class GetLeaveRequestList
    {
        public class Query : IRequest<List<LeaveRequestListDto>> { }

        public class Handler : IRequestHandler<Query, List<LeaveRequestListDto>>
        {
            private readonly ILeaveRequestRepository _repository;
            private readonly IMapper _mapper;

            public Handler(ILeaveRequestRepository repository, IMapper mapper)
            {
                _mapper = mapper;
                _repository = repository;
            }

            public async Task<List<LeaveRequestListDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var LeaveRequests = await _repository.GetLeaveRequestsWithDetails();

                return _mapper.Map<List<LeaveRequestListDto>>(LeaveRequests);
            }
        }
    }
}