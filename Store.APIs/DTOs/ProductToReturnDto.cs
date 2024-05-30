using Store.Core.Models;

namespace Store.APIs.DTOs
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int ProductBrandId { get; set; } //fk doesnt allow null
        public string ProductBrand { get; set; }
        public int ProductTypeId { get; set; } //fk
        public string ProductType { get; set; }
    }
}
