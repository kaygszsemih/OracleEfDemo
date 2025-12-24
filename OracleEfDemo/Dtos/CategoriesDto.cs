using System.ComponentModel.DataAnnotations;

namespace OracleEfDemo.Dtos
{
    public class CategoriesDto : BaseDto
    {
        [Required]
        public string CategoryName { get; set; } = null!;

        public ICollection<ProductsDto> ProductsDto { get; set; } = [];
    }
}
