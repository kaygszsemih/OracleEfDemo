namespace OracleEfDemo.Dtos
{
    public class CustomersDto : BaseDto
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public List<OrdersDto> OrdersDto { get; set; } = [];
    }
}
