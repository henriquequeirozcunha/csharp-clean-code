using Application.DTOs.LeaveRequests;
using Application.UseCases.LeaveRequests;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestsController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<List<LeaveRequestDto>>> Get()
        {
            var leaveRequests = await Mediator.Send(new GetLeaveRequestList.Query());
            return Ok(leaveRequests);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveRequestDto>> Get(int id)
        {
            var leaveRequests = await Mediator.Send(new GetLeaveRequestDetail.Query { Id = id });
            return Ok(leaveRequests);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] CreateLeaveRequestDto leaveRequest)
        {
            var response = await Mediator.Send(new CreateLeaveRequest.Command { CreateLeaveRequestDto = leaveRequest });
            //var response = await Mediator.Send(command);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateLeaveRequestDto leaveRequest)
        {
            var response = await Mediator.Send(new UpdateLeaveRequest.Command { Id = id, UpdateLeaveRequestDto = leaveRequest });

            return NoContent();
        }

        [HttpPut("changeapproval/{id}")]
        public async Task<ActionResult> ChangeApproval(int id, [FromBody] ChangeLeaveRequestApprovalDto changeLeaveRequestApprovalDto)
        {
            var response = await Mediator.Send(new UpdateLeaveRequest.Command { Id = id, ChangeLeaveRequestApprovalDto = changeLeaveRequestApprovalDto });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteLeaveRequest.Command { Id = id });

            return NoContent();
        }
    }
}