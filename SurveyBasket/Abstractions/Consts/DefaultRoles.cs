namespace SurveyBasket.Abstractions.Consts;

public static class DefaultRoles
{

    public partial class Admin {

        public const string Name = nameof(Admin);
        public const string Id = "019cb26c-19dc-72af-b5da-a6bf9a8ee67b";
        public const string ConcurrencyStamp = "019cb26d-85f0-7755-a93f-bfed39e6a0c5";

    }


    public partial class Member
    {
        public const string Name = nameof(Member);
        public const string Id = "019cb26c-19dc-72af-b5da-a6c02c75159d";
        public const string ConcurrencyStamp = "019cb26d-85f0-7755-a93f-bfeed55af204";
    }

    

}