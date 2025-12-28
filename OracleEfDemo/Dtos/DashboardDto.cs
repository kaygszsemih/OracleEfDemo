namespace OracleEfDemo.Dtos
{
    public class DashboardDto
    {
        public List<CustomerListDto> Customers { get; set; } = null!;

        public List<OrderListDto> Orders { get; set; } = null!;
    }
}
