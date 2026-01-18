
namespace SurveyBasket.Contracts.Validations;

public class CreatePollRequestValidator : AbstractValidator<CreatePollRequest>
{
    public CreatePollRequestValidator()
    {
        RuleFor(x => x.Title).Length(3, 100).
            NotEmpty().
            WithMessage("Title Should be between {MinLength} and {MaxLength}, you entered [{TotalLength}]");
        RuleFor(x => x.Description).Length(3, 1000).
            NotEmpty().
            WithMessage("Title Should be between {MinLength} and {MaxLength}, you entered [{TotalLength}]");
    }
}
