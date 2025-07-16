namespace ProductCatalog.Application.Requests
{
    public class PagedQueryRequest
    {
        public Dictionary<string, string>? Filters { get; set; } = new();
        public string? Orden { get; set; }
        public bool Asc { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
