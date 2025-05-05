using System.ComponentModel.DataAnnotations;

namespace XelerationTask.Application.DTOs
{
    public class QueryParametersDTO
    {
        public Dictionary<string, string> Filters { get; set; } = new Dictionary<string, string>();
        public string? SortBy { get; set; } = "Name";
        public bool SortDescending { get; set; } = false;

        [Range(1, int.MaxValue, ErrorMessage = "Page must be positive number.")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Page Size must be between 1 and 100.")]
        public int PageSize { get; set; } = 10;
    }
}
