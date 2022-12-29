using Application.Contracts.Persistence;
using Application.DTOs.LeaveTypes;
using Application.Exceptions;
using Application.Responses;
using Application.UseCases.LeaveTypes;
using AutoMapper;
using Shouldly;
using tests.Mocks;

namespace tests.LeaveTypes
{
    public class CreateLeaveTypeTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILeaveTypeRepository> _mockRepo;
        private readonly CreateLeaveTypeDto _leaveTypeDto;
        private readonly CreateLeaveType.Handler _sut;
        public CreateLeaveTypeTests()
        {
            _mockRepo = MockLeaveTypeRepository.GetLeaveTypeRepository();

            var mapperConfig = new MapperConfiguration(c => {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
            _sut = new CreateLeaveType.Handler(_mockRepo.Object, _mapper);

            _leaveTypeDto = new CreateLeaveTypeDto
            {
                DefaultDays = 15,
                Name = "Test DTO"
            };
        }

        [Fact]
        public async Task Should_Create_Leave_Type_On_Success()
        {
            var result = await _sut.Handle(new CreateLeaveType.Command() { LeaveTypeDto =  _leaveTypeDto}, CancellationToken.None);

            var leaveTypes = await _mockRepo.Object.GetAll();

            result.ShouldBeOfType<BaseCommandResponse>();
            leaveTypes.Count.ShouldBe(3);
        }

        [Fact]
        public async Task Should_Throw_Validation_Exception_On_Invalid_Input()
        {
            _leaveTypeDto.DefaultDays = -1;

            // Em forma de try/catch para verifica o tipo da exception
            // try
            // {
            //     result = await _sut.Handle(new CreateLeaveType.Command() { LeaveTypeDto =  _leaveTypeDto}, CancellationToken.None);

            // }
            // catch (Exception ex)
            // {
            //     ex.ShouldBeOfType<CustomValidationException>();

            //     var leaveTypes = await _mockRepo.Object.GetAll();

            //     leaveTypes.Count.ShouldBe(2);
            // }

            // Em forma de Promise retornando a exception
            // CustomValidationException ex = await Should.ThrowAsync<CustomValidationException>
            // (
            //     async () => {
            //         await _sut.Handle(new CreateLeaveType.Command() { LeaveTypeDto =  _leaveTypeDto}, CancellationToken.None);
            //     }
            // );

            // Em forma de promise
            // var promise = _sut.Handle(new CreateLeaveType.Command() { LeaveTypeDto =  _leaveTypeDto}, CancellationToken.None);

            // var ex = await Should.ThrowAsync<CustomValidationException>(promise);

            // ex.ShouldNotBeNull();

            var result = await _sut.Handle(new CreateLeaveType.Command() { LeaveTypeDto =  _leaveTypeDto}, CancellationToken.None);
            var leaveTypes = await _mockRepo.Object.GetAll();

            result.ShouldBeOfType<BaseCommandResponse>();
            leaveTypes.Count.ShouldBe(2);
        }
    }
}