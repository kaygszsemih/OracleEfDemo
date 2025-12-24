namespace OracleEfDemo.Models
{
    public class Products : BaseEntity
    {
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
        public decimal StockQuantity { get; set; }
        public int CategoryId { get; set; }

        public Categories Categories { get; set; } = null!;
        public ICollection<OrderItems> OrderItems { get; set; } = [];
    }
}
