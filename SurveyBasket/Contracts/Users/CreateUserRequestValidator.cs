using SurveyBasket.Abstractions.Consts;

namespace SurveyBasket.Contracts.Users;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3,100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(3,100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPatterns.Password)
            .WithMessage("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");


        RuleFor(x => x.Roles)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.Roles)
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("Roles must be unique.")
            .When(x => x.Roles != null);
    }
}
    