using Application.Contracts.Persistence;
using Application.DTOs.LeaveRequests;
using AutoMapper;
using MediatR;

namespace Application.UseCases.LeaveRequests
{
    public class UpdateLeaveRequest
    {
        public class Command : IRequest<Unit>
        {
            public UpdateLeaveRequestDto LeaveRequestDto { get; set; }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ILeaveRequestRepository _repository;
            private readonly IMapper _mapper;
            public Handler(ILeaveRequestRepository repository, IMapper mapper)
            {
                _mapper = mapper;
                _repository = repository;

            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var leaveRequest = await _repository.Get(request.LeaveRequestDto.Id);

                _mapper.Map(request.LeaveRequestDto, leaveRequest);

                await _repository.Upadte(leaveRequest);

                return Unit.Value;
            }
        }
    }
}