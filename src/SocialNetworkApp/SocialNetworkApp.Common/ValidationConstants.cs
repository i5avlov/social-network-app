namespace SocialNetworkApp.Common
{
    public static class ValidationConstants 
    {
        public static class ApplicationUser
        {
            public const int FirstNameMaxLength = 50;
            public const int LastNameMaxLength = 50;
            public const int CityMaxLength = 80;
            public const int OccupationMaxLength = 100;
            public const string BirthdateColumnTypeName = "DATE"; 
        }

        public static class Post
        {
            public const int TextMinLength = 2;
            public const int TextMaxLength = 1500; 
            
        }
        
    }
}
