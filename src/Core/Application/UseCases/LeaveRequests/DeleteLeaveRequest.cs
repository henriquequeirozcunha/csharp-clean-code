using Application.Contracts.Persistence;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.LeaveRequests
{
    public class DeleteLeaveRequest
    {
        public class Command : IRequest<Unit>
        {
            public int Id { get; set; }
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
                var leaveRequest = await _repository.Get(request.Id);

                if (leaveRequest == null) throw new NotFoundException(nameof(LeaveRequest), request.Id);

                await _repository.Remove(leaveRequest);

                return Unit.Value;
            }
        }
    }
}