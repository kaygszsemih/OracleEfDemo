namespace OracleEfDemo.Models
{
    public class OrderItems : BaseEntity
    {
        public string ProductName { get; set; } = null!;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public int OrderId { get; set; }
        public Orders Orders { get; set; } = null!;

        public int ProductId { get; set; }
        public Products Products { get; set; } = null!;
    }
}
