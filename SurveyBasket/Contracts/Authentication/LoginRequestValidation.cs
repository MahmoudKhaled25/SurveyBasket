using Microsoft.AspNetCore.Identity.Data;

namespace SurveyBasket.Contracts.Polls;


public class LoginRequestValidation : AbstractValidator<LoginRequest>
{
    public LoginRequestValidation()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}