using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Programlama__Proje.Models;

namespace Web_Programlama__Proje.Controllers
{
    public class HizmetlerController : Controller
    {
        RendevuContext _context=new RendevuContext();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult HizmetYazdir()
        {
           var hizmetler = _context.Hizmetler.ToList();
            return View(hizmetler);
        }
    }
}
