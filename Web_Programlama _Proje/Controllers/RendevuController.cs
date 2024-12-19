using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_Programlama__Proje.Models;
namespace Web_Programlama__Proje.Controllers
{
    public class RendevuController : Controller
    {
        RendevuContext _context = new RendevuContext();
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles ="Admin")]
        public IActionResult Rendevular()
        {
            var rendevular = _context.Rendevular.ToList();
            return View(rendevular);
        }
        public IActionResult RendevuKaydet(Rendevu rendevu)
        {
            if (ModelState.IsValid)
            {
                _context.Rendevular.Add(rendevu);
                _context.SaveChanges();
                TempData["msj"] = rendevu.MusteriAd + " : Adlı müşteri için başarıyla rendevu alınmıştır";
                return RedirectToAction("Rendevular");


            }
            TempData["msj"] = "Lütfen bilgilerin doğrulundan emin olun";
            return RedirectToAction("RendevuAl");
        }


        public IActionResult RendevuAl()
        {
            return View();
        }
    }
}
