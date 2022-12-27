using Application.Contracts.Persistence;
using Application.DTOs.LeaveAllocations;
using AutoMapper;
using MediatR;

namespace Application.UseCases.LeaveAllocations
{
    public class UpdateLeaveAllocation
    {
        public class Command : IRequest<Unit>
        {
            public UpdateLeaveAllocationDto LeaveAllocationDto { get; set; }
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
                var leaveAllocation = await _repository.Get(request.LeaveAllocationDto.Id);

                _mapper.Map(request.LeaveAllocationDto, leaveAllocation);

                await _repository.Upadte(leaveAllocation);

                return Unit.Value;
            }
        }
    }
}