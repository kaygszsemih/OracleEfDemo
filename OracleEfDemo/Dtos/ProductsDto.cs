using System.ComponentModel.DataAnnotations;

namespace OracleEfDemo.Dtos
{
    public class ProductsDto : BaseDto
    {
        [Required]
        public string ProductName { get; set; } = null!;


        [Required]
        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;
        public decimal StockQuantity { get; set; }

        [Required(ErrorMessage = "Kategori seçiniz.")]
        [Range(1, int.MaxValue, ErrorMessage = "Kategori seçiniz.")]
        public int CategoryId { get; set; }

        public CategoriesDto? CategoriesDto { get; set; }
        public ICollection<OrderItemsDto> OrderItemsDto { get; set; } = [];
    }
}
