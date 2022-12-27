using Application.Contracts.Persistence;
using Application.DTOs.LeaveAllocations;
using AutoMapper;
using MediatR;

namespace Application.UseCases.LeaveAllocation
{
    public class GetLeaveAllocationList
    {
        public class Query : IRequest<List<LeaveAllocationDto>> { }

        public class Handler : IRequestHandler<Query, List<LeaveAllocationDto>>
        {
            private readonly ILeaveAllocationRepository _repository;
            private readonly IMapper _mapper;

            public Handler(ILeaveAllocationRepository repository, IMapper mapper)
            {
                _mapper = mapper;
                _repository = repository;
            }

            public async Task<List<LeaveAllocationDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var leaveAllocations = await _repository.GetLeaveAllocationsWithDetails();

                return _mapper.Map<List<LeaveAllocationDto>>(leaveAllocations);
            }
        }
    }
}