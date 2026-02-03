using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using SurveyBasket.Contracts.Polls;

namespace SurveyBasket.Controllers;

[Route("api/[controller]")]  //api/polls
[ApiController]
[Authorize]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;

    [HttpGet("")]
    public async Task  <IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var polls = await _pollService.GetAllAsync();
        var response = polls.Adapt<IEnumerable<PollResponse>>();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task <IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var poll = await _pollService.GetAsync(id);
        if (poll is null)
        {
            return NotFound();
        }
        var response = poll.Adapt<PollResponse>();
        return Ok(response);

    }

    [HttpPost("")]

    public async Task <IActionResult> Add([FromBody] PollRequest request,CancellationToken cancellationToken)
    {
        var newPool = await _pollService.AddAsync(request.Adapt<Poll>(),cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = newPool.Id }, newPool);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request,CancellationToken cancellationToken)
    {
        var isUpdated = await _pollService.UpdateAsync(id, request.Adapt<Poll>(),cancellationToken);
        if (!isUpdated)
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id,CancellationToken cancellationToken)
    {
        var isDeleted = await _pollService.DeleteAsync(id);
        if (!isDeleted)
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpPut("{id}/togglePublish")]
    public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
    {
        var isUpdated = await _pollService.TogglePublishStatusAsync(id, cancellationToken);
        if (!isUpdated)
        {
            return NotFound();
        }
        return NoContent();
    }

}
