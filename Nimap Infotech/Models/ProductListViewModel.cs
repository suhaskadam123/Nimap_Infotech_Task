namespace Nimap_Infotech.Models
{
    public class ProductListViewModel
    {
        public List<ProductViewModel> Products { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalProducts { get; set; }
    }
}
