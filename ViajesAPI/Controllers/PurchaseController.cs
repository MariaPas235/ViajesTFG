using Microsoft.AspNetCore.Mvc;

namespace ViajesAPI.Controllers
{
    public class PurchaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
