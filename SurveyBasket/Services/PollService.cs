

using Azure.Core;
using SurveyBasket.Contracts.Polls;
using SurveyBasket.Errors;

namespace SurveyBasket.Services;

public class PollService(ApplicationDbContext context) : IPollService
{
    private readonly ApplicationDbContext _context = context;
    public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken = default) => await _context.Polls.AsNoTracking().ToListAsync();


    public async Task<Result<PollResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.FindAsync(id);
        return poll is null ? Result.Failure<PollResponse>(PollErrors.PollNotFound) : Result.Success<PollResponse>(poll.Adapt<PollResponse>());
    }


    public async Task <PollResponse> AddAsync(PollRequest request, CancellationToken cancellationToken = default)
    {
        var poll = request.Adapt<Poll>();
        await _context.Polls.AddAsync(poll,cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return poll.Adapt<PollResponse>();
    }

    public async Task<Result> UpdateAsync(int id, PollRequest poll,CancellationToken cancellationToken = default)
    {
        var currentPoll =  await _context.Polls.FindAsync(id,cancellationToken);
        if (currentPoll == null)
        {
            return Result.Failure(PollErrors.PollNotFound);
        }
        currentPoll.Title = poll.Title;
        currentPoll.Summary = poll.Summary;
        currentPoll.StartsAt = poll.StartsAt;
        currentPoll.EndsAt = poll.EndsAt;

      await  _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task <Result> DeleteAsync(int id,CancellationToken cancellationToken = default)
    {
        var currentPoll = await _context.Polls.FindAsync(id,cancellationToken);
        if (currentPoll == null)
        {
            return Result.Failure(PollErrors.PollNotFound);
        }
        _context.Remove(currentPoll);
     await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
    }
    public async Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var currentPoll = await _context.Polls.FindAsync(id, cancellationToken);
        if (currentPoll == null)
            return Result.Failure(PollErrors.PollNotFound);

        currentPoll.isPublished = !currentPoll.isPublished;
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }


}
