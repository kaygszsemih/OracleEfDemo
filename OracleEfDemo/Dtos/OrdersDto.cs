
namespace OracleEfDemo.Dtos
{
    public class OrdersDto : BaseDto
    {
        public int CustomerId { get; set; }
        public string OrderNumber { get; set; } = null!;
        public decimal Total { get; set; }

        public CustomersDto CustomersDto { get; set; } = null!;
        public List<OrderItemsDto> OrderItemsDto { get; set; } = [];
    }
}
