namespace SurveyBasket.Abstractions.Consts;

public static class DefaultRoles
{

    public partial class Admin {

        public const string Name = nameof(Admin);
        public const string Id = "019d2483-0156-7666-bb1c-7cdedab0d6fd";
        public const string ConcurrencyStamp = "019d2483-0156-7666-bb1c-7cdfc3b4ee64";

    }


    public partial class Member
    {
        public const string Name = nameof(Member);
        public const string Id = "019d2483-0156-7666-bb1c-7ce0ae4b3cc2";
        public const string ConcurrencyStamp = "019d2483-0156-7666-bb1c-7ce1be76bc65";
    }

    

}