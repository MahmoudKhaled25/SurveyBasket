namespace SurveyBasket.Abstractions.Consts;

public static class Permissions
{
    public static string Type { get; } = "permission";

    public static string GetPolls = "polls:read";
    public static string AddPolls = "polls:add";
    public static string UpdatePolls = "polls:update";
    public static string DeletePolls = "polls:delete";

    public static string GetQuestions = "questions:read";
    public static string AddQuestions = "questions:add";
    public static string DeleteQuestions = "questions:delete";

    public static string GetUsers = "users:read";
    public static string AddUsers = "users:add";
    public static string UpdateUsers = "users:update";

    public static string GetRole = "role:read";
    public static string AddRole = "role:add";
    public static string UpdateRole = "role:update";

    public static string Results = "result:read";



    public static IList<string?> GetAllPermissions() => typeof(Permissions).GetFields().Select(x => x.GetValue(x) as string).ToList();
}
