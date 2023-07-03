namespace WebApi.Models
{
    public class SearchQueryModel
    {
        public string query { get; set; } = string.Empty;

        public int page { get; set; } = 1;

        public int pageSize { get; set; } = 10;
    }
}
