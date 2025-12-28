namespace OracleEfDemo.Dtos
{
    public class CustomerListDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public decimal OrderTotal { get; set; }
    }
}
