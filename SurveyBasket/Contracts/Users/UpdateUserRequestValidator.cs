namespace SurveyBasket.Contracts.Users;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3,100)
            .WithMessage("First name is required");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(3, 100)
            .WithMessage("Last name is required");


        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Valid email is required");


        RuleFor(x => x.Roles)
            .NotEmpty()
            .NotNull()
            .WithMessage("At least one role is required");

        RuleFor(x => x.Roles)
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("Duplicate roles are not allowed")
            .When(x => x.Roles != null);
    }
}
