namespace OracleEfDemo.Models
{
    public class Orders : BaseEntity
    {
        public int CustomerId { get; set; }
        public string OrderNumber { get; set; } = null!;
        public decimal Total { get; set; }

        public Customers Customers { get; set; } = null!;
        public List<OrderItems> OrderItems { get; set; } = [];
    }
}
