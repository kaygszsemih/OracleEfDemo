namespace OracleEfDemo.Dtos
{
    public class UserDto
    {
        public string? Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Department { get; set; }
        public decimal? Salary { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
