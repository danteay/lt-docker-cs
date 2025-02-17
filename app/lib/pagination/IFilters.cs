namespace app.lib.pagination
{
    public interface IFilters<T>
    {
        IQueryable<T> Apply(IQueryable<T> query);
        int MaxPageSize();
        int PageSize();
        int PageNumber();
    }
}
