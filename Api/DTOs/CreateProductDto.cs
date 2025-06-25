using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Type is required.")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Brand is required.")]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Image URL is required.")]
        public string ImgUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
        public int StockQuantity { get; set; }
    }
}
