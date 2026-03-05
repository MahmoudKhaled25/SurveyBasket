namespace SurveyBasket.Errors;

public static class RoleErrors
{
    public static readonly Error RoleNotFound =
     new("Role.RoleNotFound", "Role Not Found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedRole =
        new("Role.DuplicatedRole", "Another Role with the same name is already exists", StatusCodes.Status409Conflict);

    public static readonly Error InvalidPermissions =
        new("Role.InvalidPermissions", "Invalid Permissions", StatusCodes.Status400BadRequest);
}
