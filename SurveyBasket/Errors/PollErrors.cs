namespace SurveyBasket.Errors;

public static class PollErrors
{
    public static readonly Error PollNotFound = new("Poll.NotFound", "Poll With Given Id is not found");

    public static readonly Error DuplicatedPollTitle = new("Poll.DuplicatedTitle", "Another Poll with the same title is already exists");

}
