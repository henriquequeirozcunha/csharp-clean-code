using Application.Contracts.Persistence;
using Application.DTOs.LeaveTypes;
using Application.Exceptions;
using Application.Responses;
using Application.UseCases.LeaveTypes.Validators;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.UseCases.LeaveTypes
{
    public class CreateLeaveType
    {
        public class Command : IRequest<BaseCommandResponse>
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

        public class Handler : IRequestHandler<Command, BaseCommandResponse>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            public Handler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
            }

            public async Task<BaseCommandResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                var validator = new CommandValidator();
                var validationResult = await validator.ValidateAsync(request.LeaveTypeDto);

                if (!validationResult.IsValid)
                {
                    return new BaseCommandResponse
                    {
                        Success = false,
                        Message = "Creation Failed",
                        Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList()
                    };
                    throw new CustomValidationException(validationResult);
                }

                var leaveType = _mapper.Map<LeaveType>(request.LeaveTypeDto);

                leaveType = await _unitOfWork.leaveTypeRepository.Add(leaveType);

                await _unitOfWork.Save();

                return new BaseCommandResponse
                {
                    Success = true,
                    Message = "Creation Successfull",
                    Id = leaveType.Id
                };
            }
        }
    }
}