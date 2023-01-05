using Application.Contracts.Persistence;
using Application.DTOs.LeaveTypes;
using Application.Exceptions;
using Application.Responses;
using Application.UseCases.LeaveTypes;
using AutoMapper;
using Domain.Entities;
using NUnit.Framework;
using Shouldly;
using tests.Mocks;

namespace tests.Core.Application.UseCases.LeaveTypes
{
    [TestFixture]
    public class CreateLeaveTypeTests
    {
        private IMapper _mapper;
        private Mock<IUnitOfWork> _mockUow;
        private CreateLeaveTypeDto _leaveTypeDto;
        private CreateLeaveType.Handler _sut;

        [SetUp]
        public void MakeSut()
        {
            _mockUow = MockUnitOfWork.GetUnitOfWork();

            var mapperConfig = new MapperConfiguration(c => {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
            _sut = new CreateLeaveType.Handler(_mockUow.Object, _mapper);

            _leaveTypeDto = new CreateLeaveTypeDto
            {
                DefaultDays = 15,
                Name = "Test DTO"
            };
        }

        [Test(Description = "Should Throws an Exception on Error")]
        public async Task should_throw_an_exception_on_error()
        {
            var mockLeaveTypeRepo = new Mock<ILeaveTypeRepository>();

            mockLeaveTypeRepo.Setup(r => r.Add(It.IsAny<LeaveType>())).ThrowsAsync(new Exception("LeaveRepositoryError"));

            _mockUow.Setup(r => r.leaveTypeRepository).Returns(mockLeaveTypeRepo.Object);

            var sut = new GetLeaveTypeDetail.Handler(_mockUow.Object.leaveTypeRepository, _mapper);

            var promise = _sut.Handle(new CreateLeaveType.Command() { LeaveTypeDto =  _leaveTypeDto}, CancellationToken.None);

            var ex = await Should.ThrowAsync<Exception>(promise);

            ex.Message.ShouldBe("LeaveRepositoryError");
            ex.ShouldBeOfType<Exception>();

            Assert.That(ex.Message, Is.EqualTo("LeaveRepositoryError"));
            Assert.That(ex, Is.TypeOf<Exception>());
        }

        [Test]
        [Ignore("Test Ignore")]
        public async Task Should_BeIgnored()
        {
            var result = await _sut.Handle(new CreateLeaveType.Command() { LeaveTypeDto =  _leaveTypeDto}, CancellationToken.None);

            var leaveTypes = await _mockUow.Object.leaveTypeRepository.GetAll();

            result.ShouldBeOfType<BaseCommandResponse>();
            leaveTypes.Count.ShouldBe(3);
        }

        [Test]
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

            var invalidCommand = new CreateLeaveType.Command() { LeaveTypeDto = new CreateLeaveTypeDto { Name = "Invalid Object" } };

            var result = await _sut.Handle(invalidCommand, CancellationToken.None);
            var leaveTypes = await _mockUow.Object.leaveTypeRepository.GetAll();

            result.ShouldBeOfType<BaseCommandResponse>();

            var expectedResult = new BaseCommandResponse {
                Success = false,
                Message = "Creation Failed",
                Errors = new List<string> { "Default Days is required" }
            };

            // TestContext.Progress.Write(JsonConvert.SerializeObject(result, Formatting.Indented));
            // TestContext.Progress.Write(JsonConvert.SerializeObject(expectedResult, Formatting.Indented));

            result.ShouldBeEquivalentTo(expectedResult);

            leaveTypes.Count.ShouldBe(2);
        }

        [Test]
        public async Task Should_Create_Leave_Type_On_Success()
        {
            var result = await _sut.Handle(new CreateLeaveType.Command() { LeaveTypeDto =  _leaveTypeDto}, CancellationToken.None);

            var leaveTypes = await _mockUow.Object.leaveTypeRepository.GetAll();

            result.ShouldBeOfType<BaseCommandResponse>();
            leaveTypes.Count.ShouldBe(3);
        }
    }
}