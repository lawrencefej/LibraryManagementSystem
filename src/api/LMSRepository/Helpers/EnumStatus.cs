namespace LMSRepository.Helpers
{
    public enum EnumStatus
    {
        Available = 1,
        Unavailable = 2,
        Checkedout = 3,
        Reserved = 4,
        Canceled = 5,
        Returned = 6,
        Expired = 7
    }

    public enum EnumRoles
    {
        Member = 1,
        Admin = 2,
        Librarian = 3
    }
}