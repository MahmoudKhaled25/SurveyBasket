using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using SurveyBasket.Contracts.Polls;
using SurveyBasket.Errors;

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
        var result = await _pollService.GetAsync(id);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem(StatusCodes.Status400BadRequest);


    }

    [HttpPost("")]

    public async Task <IActionResult> Add([FromBody] PollRequest request,CancellationToken cancellationToken)
    {
        var result = await _pollService.AddAsync(request,cancellationToken);
        return result.IsSuccess? 
            CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value) : 
            result.ToProblem(StatusCodes.Status409Conflict);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request,CancellationToken cancellationToken)
    {
        var result = await _pollService.UpdateAsync(id, request,cancellationToken);

        if (result.IsSuccess)
            return NoContent();

        return result.Error.Equals(PollErrors.DuplicatedPollTitle)
                ? result.ToProblem(StatusCodes.Status409Conflict)
                : result.ToProblem(StatusCodes.Status404NotFound);
    }
    [HttpDelete("{id}")]    
    public async Task<IActionResult> Delete([FromRoute] int id,CancellationToken cancellationToken)
    {
        var result = await _pollService.DeleteAsync(id);
      
            return result.IsSuccess ? NoContent() : result.ToProblem(StatusCodes.Status400BadRequest);
        
        
    }
    [HttpPut("{id}/togglePublish")]
    public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.TogglePublishStatusAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem(StatusCodes.Status400BadRequest);

    }

}
