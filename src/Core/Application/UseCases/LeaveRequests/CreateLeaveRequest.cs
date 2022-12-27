using Application.Contracts.Persistence;
using Application.DTOs.LeaveRequests;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.LeaveRequests
{
    public class CreateLeaveRequest
    {
        public class Command : IRequest<int>
        {
            public CreateLeaveRequestDto LeaveRequestDto { get; set; }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ILeaveRequestRepository _repository;
            private readonly IMapper _mapper;
            public Handler(ILeaveRequestRepository repository, IMapper mapper)
            {
                _mapper = mapper;
                _repository = repository;

            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var leaveRequest = _mapper.Map<LeaveRequest>(request.LeaveRequestDto);

                leaveRequest = await _repository.Add(leaveRequest);

                return leaveRequest.Id;
            }
        }
    }
}