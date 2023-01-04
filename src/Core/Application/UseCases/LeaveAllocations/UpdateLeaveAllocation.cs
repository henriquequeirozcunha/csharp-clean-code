using Application.Contracts.Persistence;
using Application.DTOs.LeaveAllocations;
using Application.Exceptions;
using Application.UseCases.LeaveAllocations.Validators;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.UseCases.LeaveAllocations
{
    public class UpdateLeaveAllocation
    {
        public class Command : IRequest<Unit>
        {
            public UpdateLeaveAllocationDto UpdateLeaveAllocationDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<UpdateLeaveAllocationDto>
        {
            public CommandValidator(ILeaveAllocationRepository repository)
            {
               Include(new ILeaveAllocationDtoValidator(repository));

               RuleFor(p => p.Id).NotNull().WithMessage("{PropertyName} must be present");
            }
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
                var validator = new CommandValidator(_unitOfWork.leaveAllocationRepository);
                var validationResult = await validator.ValidateAsync(request.UpdateLeaveAllocationDto);

                if (!validationResult.IsValid) throw new CustomValidationException(validationResult);

                var leaveAllocation = await _unitOfWork.leaveAllocationRepository.Get(request.UpdateLeaveAllocationDto.Id);

                if (leaveAllocation is null) throw new NotFoundException(nameof(leaveAllocation), request.UpdateLeaveAllocationDto.Id);

                _mapper.Map(request.UpdateLeaveAllocationDto, leaveAllocation);

                await _unitOfWork.leaveAllocationRepository.Upadte(leaveAllocation);
                await _unitOfWork.Save();

                return Unit.Value;
            }
        }
    }
}