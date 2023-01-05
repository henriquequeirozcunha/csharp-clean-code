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
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            public Handler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var leaveType = await _unitOfWork.leaveTypeRepository.Get(request.Id);

                if (leaveType == null) throw new NotFoundException(nameof(LeaveType), request.Id);

                await _unitOfWork.leaveTypeRepository.Remove(leaveType);
                await _unitOfWork.Save();

                return Unit.Value;
            }
        }
    }
}