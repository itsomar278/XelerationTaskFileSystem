namespace XelerationTask.Core.Models
{
    public class QueryResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int PageCount => (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
}
