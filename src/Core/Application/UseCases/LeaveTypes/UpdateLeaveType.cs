using Application.Contracts.Persistence;
using Application.DTOs.LeaveTypes;
using Application.UseCases.LeaveTypes.Validators;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.UseCases.LeaveTypes
{
    public class UpdateLeaveType
    {
        public class Command : IRequest<Unit>
        {
            public UpdateLeaveTypeDto LeaveTypeDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<LeaveTypeDto>
        {
            public CommandValidator()
            {
                Include(new ILeaveTypeDtoValidator());

                RuleFor(p => p.Id).NotNull().WithMessage("{PropertyName} must be present");
            }
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
                var leaveType = await _repository.Get(request.LeaveTypeDto.Id);

                _mapper.Map(request.LeaveTypeDto, leaveType);

                await _repository.Upadte(leaveType);

                return Unit.Value;
            }
        }
    }
}