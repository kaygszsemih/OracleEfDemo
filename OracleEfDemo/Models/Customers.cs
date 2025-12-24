namespace OracleEfDemo.Models
{
    public class Customers : BaseEntity
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public ICollection<Orders> Orders { get; set; } = [];
    }
}
