using Microsoft.AspNetCore.Mvc;

namespace OracleEfDemo.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
