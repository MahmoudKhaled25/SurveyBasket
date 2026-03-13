namespace SurveyBasket.Contracts.Users;

public record UpdateUserRequest
(
    string FirstName,
    string LastName,
    string Email,
    List<string> Roles
);
