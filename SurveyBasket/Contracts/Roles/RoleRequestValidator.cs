namespace SurveyBasket.Contracts.Roles;

public class RoleRequestValidator : AbstractValidator<RoleRequest>
{
    public RoleRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3,265)
            .WithMessage("Role name is required.");

        RuleFor(x => x.Permissions)
            .NotNull()
            .NotEmpty()
            .WithMessage("At least one permission is required.");


        RuleFor(x => x.Permissions)
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("Duplicate permissions are not allowed.")
            .When(x => x.Permissions != null);
    }   
}
