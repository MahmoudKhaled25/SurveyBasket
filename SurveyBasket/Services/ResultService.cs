using SurveyBasket.Contracts.Questions;
using SurveyBasket.Contracts.Results;
using SurveyBasket.Errors;

namespace SurveyBasket.Services;

public class ResultService(ApplicationDbContext context) : IResultService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<PollVotesResponse>> GetPollVotesAsync(int PollId,CancellationToken cancellationToken= default)
    {
        var pollVotes = await _context.Polls
            .Where(x => x.Id == PollId)
            .Select(x => new PollVotesResponse(
                x.Title,
                x.Votes.Select(v => new VoteResponse
                ($"{v.User.FirstName} {v.User.LastName}",
                v.SubmittedOn,
                v.VoteAnswers.Select(answer => new QuestionAnswerResponse(answer.Question.Content,answer.Answer.Content))))
                ))
            .SingleOrDefaultAsync(cancellationToken);

        return pollVotes is null ? Result.Failure<PollVotesResponse>(PollErrors.PollNotFound)
                                 : Result.Success(pollVotes);
            
    }
    public async Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesPerDayAsync(int PollId, CancellationToken cancellationToken = default)
    {
        var isPollExists = await _context.Polls.AnyAsync(x => x.Id == PollId);
        if (!isPollExists)
            return Result.Failure<IEnumerable<VotesPerDayResponse>>(PollErrors.PollNotFound);

        var votesPerDay = await _context.Votes
                            .Where(x => x.PollId == PollId)
                            .GroupBy(x => new {date = DateOnly.FromDateTime(x.SubmittedOn)})
                            .Select(g => new VotesPerDayResponse(
                                g.Key.date,
                                g.Count()))
                            .ToListAsync(cancellationToken); 

            return Result.Success<IEnumerable<VotesPerDayResponse>>(votesPerDay);

    }
    public async Task<Result<IEnumerable<VotesPerQuestionResponse>>>GetVotesPerQuestionAsync(int PollId, CancellationToken cancellationToken = default)
    {
        var isPollExists = await _context.Polls.AnyAsync(x => x.Id == PollId);
        if (!isPollExists)
            return Result.Failure<IEnumerable<VotesPerQuestionResponse>>(PollErrors.PollNotFound);

        var votesPerQuestion = await _context.VoteAnswers
                                .Where(x => x.Vote.PollId == PollId)
                                .Select(x => new VotesPerQuestionResponse(
                                    x.Question.Content,
                                    x.Question.Votes
                                    .GroupBy(x => new {Answers =x.Answer.Id,AnswerContent = x.Answer.Content})
                                    .Select(g => new VotesPerAnswerResponse(g.Key.AnswerContent,g.Count()))
                                    )).ToListAsync(cancellationToken);

        return Result.Success<IEnumerable<VotesPerQuestionResponse>>(votesPerQuestion);
    }
}
