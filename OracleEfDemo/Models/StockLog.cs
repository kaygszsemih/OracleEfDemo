namespace OracleEfDemo.Models
{
    public class StockLog : BaseEntity
    {
        public string ProductName { get; set; } = null!;

        public decimal StockQuantity { get; set; }

        public string UserName { get; set; } = null!;
    }
}
