namespace LMSEntities.Helpers
{
    public static class PagedListMapper<TList1, TList2>
    {
        public static PagedList<TList1> MapPagedList(PagedList<TList1> pageTo, PagedList<TList2> pageFrom)
        {
            pageTo.CurrentPage = pageFrom.CurrentPage;
            pageTo.PageSize = pageFrom.PageSize;
            pageTo.TotalCount = pageFrom.TotalCount;
            pageTo.TotalPages = pageFrom.TotalPages;

            return pageTo;
        }
    }
}
