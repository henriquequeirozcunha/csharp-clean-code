using Application.Contracts.Persistence;
using Application.DTOs.LeaveAllocations;
using AutoMapper;
using MediatR;

namespace Application.UseCases.LeaveAllocations
{
    public class GetLeaveAllocationDetail
    {
        public class Query : IRequest<LeaveAllocationDto>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, LeaveAllocationDto>
        {
            private readonly ILeaveAllocationRepository _repository;
            private readonly IMapper _mapper;
            public Handler(ILeaveAllocationRepository repository, IMapper mapper)
            {
                _mapper = mapper;
                _repository = repository;
            }

            public async Task<LeaveAllocationDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var leaveAllocation = await _repository.GetLeaveAllocationWithDetails(request.Id);

                return _mapper.Map<LeaveAllocationDto>(leaveAllocation);
            }
        }
    }
}