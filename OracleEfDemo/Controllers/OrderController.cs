using Microsoft.AspNetCore.Mvc;

namespace OracleEfDemo.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
