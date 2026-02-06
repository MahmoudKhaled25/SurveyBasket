namespace SurveyBasket.Errors;

public static class PollErrors
{
    public static readonly Error PollNotFound = new("Poll.NotFound", "Poll With Given Id is not found");
}
