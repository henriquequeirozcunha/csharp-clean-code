using Application.Contracts.Persistence;
using Application.DTOs.LeaveRequests;
using AutoMapper;
using MediatR;

namespace Application.UseCases.LeaveRequests
{
    public class GetLeaveRequestDetail
    {
        public class Query : IRequest<LeaveRequestDto>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, LeaveRequestDto>
        {
            private readonly ILeaveRequestRepository _repository;
            private readonly IMapper _mapper;
            public Handler(ILeaveRequestRepository repository, IMapper mapper)
            {
                _mapper = mapper;
                _repository = repository;
            }

            public async Task<LeaveRequestDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var leaveRequest = await _repository.GetLeaveRequestWithDetails(request.Id);

                return _mapper.Map<LeaveRequestDto>(leaveRequest);
            }
        }
    }
}