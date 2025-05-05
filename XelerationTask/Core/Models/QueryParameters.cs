namespace XelerationTask.Core.Models
{
    public class QueryParameters
    {
        public Dictionary<string, string> Filters { get; set; } = new Dictionary<string, string>();
        public string? SortBy { get; set; } = "Id";
        public bool SortDescending { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
