using Microsoft.AspNetCore.Mvc;

namespace Web_Programlama__Proje.Controllers
{
    public class RestApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
