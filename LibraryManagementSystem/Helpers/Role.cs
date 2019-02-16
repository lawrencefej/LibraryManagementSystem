using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.API.Helpers
{
    public static class Role
    {
        public const string Admin = "Admin";
        public const string Member = "Member";
        public const string Librarian = "Librarian";
        public const string RequireAdminRole = "RequireAdminRole";
        public const string RequireMemberRole = "RequireLibrarianRole";
        public const string RequireLibrarianRole = "RequireMemberRole";
    }
}