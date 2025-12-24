using Microsoft.AspNetCore.Identity;

namespace OracleEfDemo.Models
{
    public class UserApp : IdentityUser<string>
    {
        public string FullName { get; set; } = null!;
        public string? Department { get; set; }
        public decimal Salary { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
