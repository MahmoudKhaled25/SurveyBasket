using SurveyBasket.Contracts.Questions;

namespace SurveyBasket.Services;

public interface IQuestionService
{
    Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int PollId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<QuestionResponse>>> GetAvaliableAsync(int PollId,string UserId, CancellationToken cancellationToken = default);
    Task<Result<QuestionResponse>> GetAsync(int PollId,int id ,CancellationToken cancellationToken = default);
    Task<Result<QuestionResponse>> AddAsync(int PollId, QuestionRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int PollId,int id, QuestionRequest request, CancellationToken cancellationToken = default);  
    Task<Result> ToggleStatusAsync(int PollId,int id,CancellationToken cancellationToken = default);



}
