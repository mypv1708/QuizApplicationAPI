namespace QuizApplicationAPI.DTOs.Responsse
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
} 