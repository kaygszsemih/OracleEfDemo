namespace OracleEfDemo.Models
{
    public class StockLog : BaseEntity
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public decimal QuantityChange { get; set; }

        public decimal StockAfter { get; set; }

        public string UserName { get; set; } = null!;
    }
}
