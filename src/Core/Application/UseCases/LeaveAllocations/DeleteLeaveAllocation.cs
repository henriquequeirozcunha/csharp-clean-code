using Application.Contracts.Persistence;
using AutoMapper;
using MediatR;

namespace Application.UseCases.LeaveAllocations
{
    public class DeleteLeaveAllocation
    {
        public class Command : IRequest<Unit>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ILeaveAllocationRepository _repository;
            private readonly IMapper _mapper;
            public Handler(ILeaveAllocationRepository repository, IMapper mapper)
            {
                _mapper = mapper;
                _repository = repository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var leaveAllocation = await _repository.Get(request.Id);

                await _repository.Remove(leaveAllocation);

                return Unit.Value;
            }
        }
    }
}