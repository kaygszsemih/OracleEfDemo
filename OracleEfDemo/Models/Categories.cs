namespace OracleEfDemo.Models
{
    public class Categories : BaseEntity
    {
        public string CategoryName { get; set; } = null!;

        public ICollection<Products> Products { get; set; } = [];
    }
}
