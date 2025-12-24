namespace OracleEfDemo.Dtos
{
    public class StockLogDto : BaseDto
    {
        public string ProductName { get; set; } = null!;

        public decimal StockQuantity { get; set; }

        public string UserName { get; set; } = null!;
    }
}
