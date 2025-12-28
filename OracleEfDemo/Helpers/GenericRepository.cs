using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using OracleEfDemo.DbContext;
using OracleEfDemo.Dtos;

namespace OracleEfDemo.Helpers
{
    public class GenericRepository
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<T> GetCustomerList<T>() where T : class
        {
            var query = @"SELECT
                            ""Id"",
                            ""FullName"",
                            ""Email"",
                            ""Phone"",
                            GET_CUSTOMER_TOTAL(""Id"") AS ""OrderTotal""
                        FROM CUSTOMERS";

            return _context.Database.SqlQueryRaw<T>(query).AsEnumerable().ToList();
        }

        public void SetOracleUserName(string userName)
        {
            var query = @"
                BEGIN 
                    DBMS_SESSION.SET_IDENTIFIER(:p_user_name); 
                END;";

            _context.Database.ExecuteSqlRaw(query, new OracleParameter("p_user_name", userName));
        }

        public (bool result, string message) TryUpdateStock(int productId, decimal quantity)
        {
            try
            {
                _context.Database.ExecuteSqlRaw(
                    "BEGIN UPDATE_STOCK(:P_PRODUCT_ID, :P_QTY_CHANGE); END;",
                    new OracleParameter("P_PRODUCT_ID", productId),
                    new OracleParameter("P_QTY_CHANGE", quantity)
                );

                return (true, "Ürün stoğu başarıyla güncellendi.");
            }
            catch (OracleException ex)
            {
                return (false, $"Stok güncelleme hatası: {ex.Message}");
            }
        }

        public (decimal totalSalary, bool result, string message) TryUpdateEmployeesSalary(decimal salaryRate)
        {
            try
            {
                _context.Database.ExecuteSqlRaw(
                    "BEGIN PKG_SALARY.adjust_salaries(:p_percent); END;",
                    new OracleParameter("p_percent", salaryRate)
                );

                var totalSalary = _context.Database.SqlQueryRaw<decimal>("SELECT PKG_SALARY.get_total_salary FROM DUAL").AsEnumerable().First();

                return (totalSalary, true, "Çalışan maaşları başarıyla güncellendi.");
            }
            catch (Exception ex)
            {
                return (0m, false, $"Maaş güncelleme hatası: {ex.Message}");
            }
        }

        public List<OrderListDto> GetCustomerOrders(int customerId)
        {
            var query = @"SELECT * FROM EFUSER.V_CUSTOMER_ORDERS WHERE ""CustomerId"" = :id";

            return _context.Database.SqlQueryRaw<OrderListDto>(query, new OracleParameter("id", customerId)).AsEnumerable().ToList();
        }
    }
}
