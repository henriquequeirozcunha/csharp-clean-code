using Application.DTOs.LeaveTypes;
using Application.Responses;
using Application.UseCases.LeaveTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeaveTypesController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<List<LeaveTypeDto>>> Get()
        {
            var leaveTypes = await Mediator.Send(new GetLeaveTypeList.Query());
            return Ok(leaveTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveTypeDto>> Get(int id)
        {
            var leaveTypes = await Mediator.Send(new GetLeaveTypeDetail.Query { Id = id });
            return Ok(leaveTypes);
        }

        [HttpPost]
        public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreateLeaveTypeDto leaveType)
        {
            var response = await Mediator.Send(new CreateLeaveType.Command { LeaveTypeDto = leaveType });

            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] UpdateLeaveTypeDto leaveType)
        {
            var response = await Mediator.Send(new UpdateLeaveType.Command { UpdateLeaveTypeDto = leaveType });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteLeaveType.Command { Id = id });

            return NoContent();
        }
    }
}