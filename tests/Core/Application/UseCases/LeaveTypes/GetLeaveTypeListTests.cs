using Application.Contracts.Persistence;
using Application.DTOs.LeaveTypes;
using Application.UseCases.LeaveTypes;
using AutoMapper;
using NUnit.Framework;
using Shouldly;
using tests.Mocks;

namespace tests.Core.Application.UseCases.LeaveTypes
{
    [TestFixture(Description = "GetLeaveTypeListTests")]
    public class GetLeaveTypeListTests
    {
        private IMapper _mapper;
        private Mock<ILeaveTypeRepository> _mockLeaveTypeRepo;

        [SetUp]
        public void MakeSut()
        {
            _mockLeaveTypeRepo = MockLeaveTypeRepository.GetLeaveTypeRepository();

            var mapperConfig = new MapperConfiguration(c => {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Test(Description = "Should call leaveTypeRepository.GetAll with correct params")]
        public async Task should_call_leave_type_repository_get_all_with_correct_params()
        {
            var sut = new GetLeaveTypeList.Handler(_mockLeaveTypeRepo.Object, _mapper);

            var result = await sut.Handle(new GetLeaveTypeList.Query(), CancellationToken.None);

            _mockLeaveTypeRepo.Verify(repo => repo.GetAll());
        }

        [Test(Description = "Should Throws an Exception on Error")]
        public async Task should_throw_an_exception_on_error()
        {
            _mockLeaveTypeRepo.Setup(repo => repo.GetAll()).Throws(new Exception("Error Exception"));

            var sut = new GetLeaveTypeList.Handler(_mockLeaveTypeRepo.Object, _mapper);

            var promise = sut.Handle(new GetLeaveTypeList.Query(), CancellationToken.None);

            var ex = await Should.ThrowAsync<Exception>(promise);

            ex.Message.ShouldBe("Error Exception");

            Assert.That(ex.Message, Is.EqualTo("Error Exception"));
            Assert.That(ex, Is.TypeOf<Exception>());
        }

        [Test(Description = "Should return a list of LeaveType on Success")]
        public async Task should_return_a_list_of_leave_type_on_success()
        {
            var sut = new GetLeaveTypeList.Handler(_mockLeaveTypeRepo.Object, _mapper);

            var result = await sut.Handle(new GetLeaveTypeList.Query(), CancellationToken.None);

            result.ShouldBeOfType<List<LeaveTypeDto>>();

            //TestContext.Progress.Write(JsonConvert.SerializeObject(result, Formatting.Indented));

            result.Count.ShouldBe(2);
        }
    }
}