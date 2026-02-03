namespace SurveyBasket.Contracts.Polls;


public class CreatePollRequestValidator : AbstractValidator<PollRequest>
{
    public CreatePollRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Summary).NotEmpty().MaximumLength(1500);

        RuleFor(x => x.StartsAt).NotEmpty().GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));
        RuleFor(x => x).Must(HasValidDates).WithName(nameof(PollRequest.EndsAt)).WithMessage("{PropertyName} should be greater than or equals start date");
    }
    private bool HasValidDates(PollRequest pollRequest) => pollRequest.EndsAt >= pollRequest.StartsAt;

}