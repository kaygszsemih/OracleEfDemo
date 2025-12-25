namespace OracleEfDemo.Dtos
{
    public class OrderItemsDto : BaseDto
    {
        public string ProductName { get; set; } = null!;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public ProductsDto? ProductsDto { get; set; }
    }
}
