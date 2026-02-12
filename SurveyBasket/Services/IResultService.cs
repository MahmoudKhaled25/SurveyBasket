using SurveyBasket.Contracts.Results;

namespace SurveyBasket.Services;

public interface IResultService
{
    Task<Result<PollVotesResponse>> GetPollVotesAsync(int PollId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesPerDayAsync(int PollId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerQuestionAsync(int PollId, CancellationToken cancellationToken = default);
}
