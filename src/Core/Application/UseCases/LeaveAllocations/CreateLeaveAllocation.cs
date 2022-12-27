using Application.Contracts.Persistence;
using Application.DTOs.LeaveAllocations;
using Application.Exceptions;
using Application.UseCases.LeaveAllocations.Validators;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.UseCases.LeaveAllocations
{
    public class CreateLeaveAllocation
    {
        public class Command : IRequest<int>
        {
            public CreateLeaveAllocationDto CreateLeaveAllocationDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<CreateLeaveAllocationDto>
        {
            public CommandValidator(ILeaveAllocationRepository repository)
            {
               Include(new ILeaveAllocationDtoValidator(repository));
            }
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
                var validator = new CommandValidator(_repository);
                var validationResult = await validator.ValidateAsync(request.CreateLeaveAllocationDto);

                if (!validationResult.IsValid) throw new CustomValidationException(validationResult);

                var leaveAllocation = _mapper.Map<LeaveAllocation>(request.CreateLeaveAllocationDto);

                leaveAllocation = await _repository.Add(leaveAllocation);

                return leaveAllocation.Id;
            }
        }
    }
}