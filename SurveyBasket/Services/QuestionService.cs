using SurveyBasket.Contracts.Answers;
using SurveyBasket.Contracts.Questions;
using SurveyBasket.Entities;
using SurveyBasket.Errors;

namespace SurveyBasket.Services;

public class QuestionService(ApplicationDbContext context) : IQuestionService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int PollId, CancellationToken cancellationToken = default)
    {
        var isPollExists = await _context.Polls.AnyAsync(x => x.Id == PollId);
        if (!isPollExists)
            return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);
        
        var questions = await _context.Questions.Where(x => x.PollId == PollId)
                                                .Include(x => x.Answers)
                                                //.Select(q => new QuestionResponse(q.Id
                                                //                                ,q.Content
                                                //                                ,q.Answers.Select(a => new AnswerResponse(a.Id,a.Content))))
                                                .ProjectToType<QuestionResponse>()
                                                .AsNoTracking()
                                                .ToListAsync(cancellationToken);

        return Result.Success<IEnumerable<QuestionResponse>>(questions);
    }

    public async Task<Result<QuestionResponse>> GetAsync(int PollId, int id, CancellationToken cancellationToken = default)
    {
        var question  = await _context.Questions.Where(x => x.PollId == PollId && x.Id == id)
                                                .Include(x => x.Answers)
                                                //.Select(q => new QuestionResponse(q.Id
                                                //                                ,q.Content
                                                //                                ,q.Answers.Select(a => new AnswerResponse(a.Id,a.Content))))
                                                .ProjectToType<QuestionResponse>()
                                                .AsNoTracking()
                                                .SingleOrDefaultAsync(cancellationToken);

        return question is not null ? Result.Success<QuestionResponse>(question) : Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);
    }
    public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        var pollIsExists = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken: cancellationToken);
        if (!pollIsExists)
            return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);

        var questionIsExists = await _context.Questions.AnyAsync(x => x.Content == request.Content && x.PollId == pollId, cancellationToken: cancellationToken);
        if(questionIsExists)
            return Result.Failure<QuestionResponse>(QuestionErrors.DuplicatedQuestionContent);


        var question = request.Adapt<Question>();
        question.PollId = pollId;

        await _context.AddAsync(question,cancellationToken);    
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(question.Adapt<QuestionResponse>());

    }

    public async Task<Result> UpdateAsync(int PollId, int id, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        var isQuestionExists = await _context.Questions.AnyAsync(x => x.PollId == PollId &&
                                                                 x.Id != id && 
                                                                 x.Content == request.Content, cancellationToken: cancellationToken);
        if (isQuestionExists)
            return Result.Failure(QuestionErrors.DuplicatedQuestionContent);
        var question = await _context.Questions.Include(x => x.Answers)
                                               .SingleOrDefaultAsync(x => x.PollId == PollId && x.Id == id, cancellationToken);
        if(question is null)
            return Result.Failure(QuestionErrors.QuestionNotFound);

        question.Content = request.Content;
        
        // current answers
        var currentAnswers = question.Answers.Select(x => x.Content).ToList();

        // add new answers
        var newAnswers = request.Answers.Except(currentAnswers).ToList();


        newAnswers.ForEach(answer =>
        {
            question.Answers.Add(new Answer { Content = answer });
        });

        question.Answers.ToList().ForEach(answer =>
        {
            answer.IsActive = request.Answers.Contains(answer.Content);
        });

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();

    }
    public async Task<Result> ToggleStatusAsync(int PollId, int id, CancellationToken cancellationToken = default)
    {
        var question = await _context.Questions.SingleOrDefaultAsync(x => x.Id==id && x.PollId == PollId,  cancellationToken: cancellationToken);
        if (question is null)
            return Result.Failure(QuestionErrors.QuestionNotFound);
        question.IsActive = !question.IsActive;
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();

    }

 
}
