using Application.Contracts.Persistence;
using Application.DTOs.LeaveTypes;
using Application.Exceptions;
using Application.UseCases.LeaveTypes.Validators;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.UseCases.LeaveTypes
{
    public class CreateLeaveType
    {
        public class Command : IRequest<int>
        {
            public CreateLeaveTypeDto LeaveTypeDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<CreateLeaveTypeDto>
        {
            public CommandValidator()
            {
                Include(new ILeaveTypeDtoValidator());
            }
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
                var validator = new CommandValidator();
                var validationResult = await validator.ValidateAsync(request.LeaveTypeDto);

                if (!validationResult.IsValid) throw new CustomValidationException(validationResult);

                var leaveType = _mapper.Map<LeaveType>(request.LeaveTypeDto);

                leaveType = await _repository.Add(leaveType);

                return leaveType.Id;
            }
        }
    }
}