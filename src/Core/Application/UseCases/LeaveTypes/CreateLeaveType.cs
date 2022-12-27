using Application.Contracts.Persistence;
using Application.DTOs.LeaveTypes;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.LeaveTypes
{
    public class CreateLeaveType
    {
        public class Command : IRequest<int>
        {
            public LeaveTypeDto LeaveTypeDto { get; set; }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ILeaveTypeRepository _repository;
            private readonly IMapper _mapper;
            public Handler(ILeaveTypeRepository repository, IMapper mapper)
            {
                _mapper = mapper;
                _repository = repository;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var leaveType = _mapper.Map<LeaveType>(request.LeaveTypeDto);

                leaveType = await _repository.Add(leaveType);

                return leaveType.Id;
            }
        }
    }
}