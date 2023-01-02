using System.Security.Claims;
using Application.Contracts.Gateways;
using Application.Contracts.Persistence;
using Application.DTOs.LeaveRequests;
using Application.Exceptions;
using Application.Models;
using Application.Responses;
using Application.UseCases.LeaveRequests.Validators;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.LeaveRequests
{
    public class CreateLeaveRequest
    {
        public class Command : IRequest<BaseCommandResponse>
        {
            public CreateLeaveRequestDto CreateLeaveRequestDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<CreateLeaveRequestDto>
        {
            public CommandValidator(ILeaveRequestRepository repository)
            {
               Include(new ILeaveRequestDtoValidator(repository));
            }
        }

        public class Handler : IRequestHandler<Command, BaseCommandResponse>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ILeaveRequestRepository _leaveRequestRepository;
            private readonly ILeaveAllocationRepository _leaveAllocationRepository;
            private readonly IMapper _mapper;
            private readonly IEmailSender _emailSender;
            public Handler(
                IHttpContextAccessor httpContextAccessor,
                ILeaveRequestRepository leaveRequestRepository,
                ILeaveAllocationRepository leaveAllocationRepository,
                IEmailSender emailSender,
                IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _mapper = mapper;
                _leaveRequestRepository = leaveRequestRepository;
                _leaveAllocationRepository = leaveAllocationRepository;
                _emailSender = emailSender;
            }

            public async Task<BaseCommandResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                var response = new BaseCommandResponse();
                var validator = new CommandValidator(_leaveRequestRepository);
                var validationResult = await validator.ValidateAsync(request.CreateLeaveRequestDto);
                var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
                var allocation = await _leaveAllocationRepository.GetUserAllocations(userId, request.CreateLeaveRequestDto.LeaveTypeId);

                int daysRequested = (int)(request.CreateLeaveRequestDto.EndDate - request.CreateLeaveRequestDto.StartDate).TotalDays;

                if (daysRequested > allocation.NumberOfDays)
                {
                    validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(nameof(request.CreateLeaveRequestDto.EndDate), "You do not have enough days for this request"));
                }

                if (!validationResult.IsValid)
                {
                    response.Success = false;
                    response.Message = "Creation Failed";
                    response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

                     throw new CustomValidationException(validationResult);
                }

                var leaveRequest = _mapper.Map<LeaveRequest>(request.CreateLeaveRequestDto);

                leaveRequest.RequestingEmployeeId = userId;

                leaveRequest = await _leaveRequestRepository.Add(leaveRequest);

                response.Success = false;
                response.Message = "Creation Successfull";
                response.Id = leaveRequest.Id;

                var emailAddress = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

                var email = new Email
                {
                    To = "employee@org.com",
                    Body = $"Your Leave Request for {request.CreateLeaveRequestDto.StartDate:D} to {request.CreateLeaveRequestDto.EndDate:D}",
                    Subject = "Leave Request submitted"
                };

                try
                {
                    await _emailSender.SendEmail(email);
                }
                catch (Exception)
                {
                    // LOG ERROR BUT DO NOT THROW EX
                }

                return response;
            }
        }
    }
}