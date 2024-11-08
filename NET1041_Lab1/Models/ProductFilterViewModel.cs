namespace NET1041_Lab1.Models
{
    public class ProductFilterViewModel
    {
        public string Keyword { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; } = "asc"; // asc or desc
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;

        public List<Product> Products { get; set; }
    }
}
