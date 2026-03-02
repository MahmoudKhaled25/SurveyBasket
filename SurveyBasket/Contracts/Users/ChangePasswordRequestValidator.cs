using SurveyBasket.Abstractions.Consts;

namespace SurveyBasket.Contracts.Users;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty();


        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .Matches(RegexPatterns.Password)
            .WithMessage("Password Should be a least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase.")
            .NotEqual(x => x.CurrentPassword)
            .WithMessage("New Password Cannot Be Same As The Current Password.");
    }

}
