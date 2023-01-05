using Application.Contracts.Persistence;
using Application.DTOs.LeaveTypes;
using Application.UseCases.LeaveTypes;
using AutoMapper;
using NUnit.Framework;
using Shouldly;
using tests.Mocks;

namespace tests.Core.Application.UseCases.LeaveTypes
{
    [TestFixture(Description = "GetLeaveTypeDetailTests")]
    public class GetLeaveTypeDetailTests
    {
        private IMapper _mapper;
        private Mock<ILeaveTypeRepository> _mockLeaveTypeRepo;
        private GetLeaveTypeDetail.Handler _sut;

        [SetUp]
        public void MakeSut()
        {
            _mockLeaveTypeRepo = MockLeaveTypeRepository.GetLeaveTypeRepository();

            var mapperConfig = new MapperConfiguration(c => {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();

            _sut = new GetLeaveTypeDetail.Handler(_mockLeaveTypeRepo.Object, _mapper);
        }

        [Test(Description = "Should call leaveTypeRepository Get with correct params")]
        public async Task should_call_leave_type_repository_get_with_correct_params()
        {
            var request = new GetLeaveTypeDetail.Query { Id =  1 };

            var result = await _sut.Handle(request, CancellationToken.None);

            _mockLeaveTypeRepo.Verify(repo => repo.Get(request.Id), Times.Once);
        }

        [Test(Description = "Should Throws an Exception on Error")]
        public async Task should_throw_an_exception_on_error()
        {
            var request = new GetLeaveTypeDetail.Query { Id =  1 };

            _mockLeaveTypeRepo.Setup(repo => repo.Get(It.IsAny<int>())).Throws(new Exception("Error Exception"));

            var sut = new GetLeaveTypeDetail.Handler(_mockLeaveTypeRepo.Object, _mapper);

            var promise = _sut.Handle(new GetLeaveTypeDetail.Query(), CancellationToken.None);

            var ex = await Should.ThrowAsync<Exception>(promise);

            ex.Message.ShouldBe("Error Exception");
            ex.ShouldBeOfType<Exception>();

            Assert.That(ex.Message, Is.EqualTo("Error Exception"));
            Assert.That(ex, Is.TypeOf<Exception>());
        }

        [Test(Description = "Should return a LeaveType with Detail on Success")]
        public async Task should_return_a_list_of_leave_type_on_success()
        {
            var request = new GetLeaveTypeDetail.Query { Id =  1 };

            var sut = new GetLeaveTypeDetail.Handler(_mockLeaveTypeRepo.Object, _mapper);

            var result = await _sut.Handle(request, CancellationToken.None);

            //TestContext.Progress.Write(JsonConvert.SerializeObject(result, Formatting.Indented));

            var expectedResult = new LeaveTypeDto  {
                Id = 99,
                DefaultDays = 99,
                Name = "Any Name"
            };

            //TestContext.Progress.Write(JsonConvert.SerializeObject(expectedResult, Formatting.Indented));

            result.ShouldBeEquivalentTo(expectedResult);
        }
    }
}