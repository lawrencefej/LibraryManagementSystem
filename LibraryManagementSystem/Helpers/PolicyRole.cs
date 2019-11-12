namespace LibraryManagementSystem.API.Helpers
{
    public static class PolicyRole
    {
        public const string Admin = "Admin";
        public const string Member = "Member";
        public const string Librarian = "Librarian";
        public const string RequireAdminRole = "RequireAdminRole";
        public const string RequireMemberRole = "RequireLibrarianRole";
        public const string RequireLibrarianRole = "RequireMemberRole";
    }
}