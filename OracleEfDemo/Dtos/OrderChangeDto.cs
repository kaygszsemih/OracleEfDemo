namespace OracleEfDemo.Dtos
{
    public class OrderListDto
    {
        public int? Id { get; set; }
        public string? OrderNumber { get; set; }
        public string? CustomerName { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
