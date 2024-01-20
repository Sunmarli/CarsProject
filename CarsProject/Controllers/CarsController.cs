using Microsoft.AspNetCore.Mvc;

namespace CarsProject.Controllers
{
    public class CarsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
