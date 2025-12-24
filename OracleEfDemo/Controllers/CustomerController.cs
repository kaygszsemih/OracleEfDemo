using Microsoft.AspNetCore.Mvc;

namespace OracleEfDemo.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
