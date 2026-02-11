namespace SurveyBasket.Errors;

public static class VoteErrors
{
    public static readonly Error InvalidQuestions = new("Question.InvalidQuestion", "Invalid Question.", StatusCodes.Status400BadRequest);

    public static readonly Error DuplicatedVote = new("Vote.DuplicatedContent", "This user has already voted for this Poll before.", StatusCodes.Status409Conflict);
}
