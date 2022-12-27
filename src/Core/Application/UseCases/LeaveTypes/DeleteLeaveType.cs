using Application.Contracts.Persistence;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.LeaveTypes
{
    public class DeleteLeaveType
    {
        public class Command : IRequest<Unit>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ILeaveTypeRepository _repository;
            private readonly IMapper _mapper;
            public Handler(ILeaveTypeRepository repository, IMapper mapper)
            {
                _mapper = mapper;
                _repository = repository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var leaveType = await _repository.Get(request.Id);

                if (leaveType == null) throw new NotFoundException(nameof(LeaveType), request.Id);

                await _repository.Remove(leaveType);

                return Unit.Value;
            }
        }
    }
}