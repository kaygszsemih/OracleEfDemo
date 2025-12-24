
namespace OracleEfDemo.Dtos
{
    public class OrderItemsDto : BaseDto
    {
        public string ProductName { get; set; } = null!;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public OrdersDto OrdersDto { get; set; } = null!;
        public ProductsDto ProductsDto { get; set; } = null!;
    }
}
