using Application.Contracts.Persistence;
using Application.DTOs.LeaveAllocations;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.LeaveAllocations
{
    public class CreateLeaveAllocation
    {
        public class Command : IRequest<int>
        {
            public CreateLeaveAllocationDto CreateLeaveAllocationDto { get; set; }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ILeaveAllocationRepository _repository;
            private readonly IMapper _mapper;
            public Handler(ILeaveAllocationRepository repository, IMapper mapper)
            {
                _mapper = mapper;
                _repository = repository;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var leaveAllocation = _mapper.Map<LeaveAllocation>(request.CreateLeaveAllocationDto);

                leaveAllocation = await _repository.Add(leaveAllocation);

                return leaveAllocation.Id;
            }
        }
    }
}