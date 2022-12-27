using Application.Contracts.Persistence;
using Application.DTOs.LeaveTypes;
using Application.Exceptions;
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
            public UpdateLeaveTypeDto UpdateLeaveTypeDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<UpdateLeaveTypeDto>
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
                var validator = new CommandValidator();
                var validationResult = await validator.ValidateAsync(request.UpdateLeaveTypeDto);

                if (!validationResult.IsValid) throw new CustomValidationException(validationResult);

                var leaveType = await _repository.Get(request.UpdateLeaveTypeDto.Id);

                _mapper.Map(request.UpdateLeaveTypeDto, leaveType);

                await _repository.Upadte(leaveType);

                return Unit.Value;
            }
        }
    }
}