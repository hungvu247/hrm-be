namespace human_resource_management.Model
{
    public class PagedResult<T>
    {
        public int TotalCount { get; set; }
        public List<T> Items { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

}
