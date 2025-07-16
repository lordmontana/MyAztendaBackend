namespace ProductService.Entities
{
    public class Product
    {
        public int Id { get; set; } // Primary key
        public string? Name { get; set; } = string.Empty; // Name of the product
        public decimal? Price { get; set; } // Price of the product
        public int? Stock { get; set; } // Stock quantity
		public string? Description { get; set; } = string.Empty; // Description of the product

        public int? IId { get; set; } 

    }
}
