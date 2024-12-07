using Microsoft.AspNetCore.Mvc;

namespace Web_Programlama__Proje.Controllers
{
    public class PersonelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult OnayVer()
        {
            return View();
        }
    }
}
