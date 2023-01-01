using Application.DTOs.LeaveAllocations;
using Application.Responses;
using Application.UseCases.LeaveAllocations;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveAllocationsController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<List<LeaveAllocationDto>>> Get()
        {
            var leaveAllocations = await Mediator.Send(new GetLeaveAllocationList.Query());
            return Ok(leaveAllocations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveAllocationDto>> Get(int id)
        {
            var leaveAllocations = await Mediator.Send(new GetLeaveAllocationDetail.Query { Id = id });
            return Ok(leaveAllocations);
        }

        [HttpPost]
        public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreateLeaveAllocationDto leaveAllocation)
        {
            var response = await Mediator.Send(new CreateLeaveAllocation.Command { CreateLeaveAllocationDto = leaveAllocation });
            //var response = await Mediator.Send(command);

            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] UpdateLeaveAllocationDto leaveAllocation)
        {
            var response = await Mediator.Send(new UpdateLeaveAllocation.Command { UpdateLeaveAllocationDto = leaveAllocation });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteLeaveAllocation.Command { Id = id });

            return NoContent();
        }
    }
}