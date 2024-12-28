using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Programlama__Proje.Models;

namespace Web_Programlama__Proje.Controllers
{
    public class HizmetlerController : Controller
    {
        private readonly RendevuContext _context = new RendevuContext();

        // Hizmetler Listesi
        public IActionResult HizmetYazdir()
        {
            var hizmetler = _context.Hizmetler.ToList();
            return View(hizmetler);
        }
        public IActionResult Index()
        {
            var hizmetler = _context.Hizmetler.ToList();
            return View(hizmetler);
        }

        // Hizmet Ekleme Sayfası (GET)
        [HttpGet]
        public IActionResult HizmetEkle()
        {
            return View();
        }

        // Hizmet Ekleme İşlemi (POST)
        [HttpPost]
        public IActionResult HizmetEkle(Hizmetler hizmet)
        {
            if (ModelState.IsValid)
            {
                _context.Hizmetler.Add(hizmet);
                _context.SaveChanges();
                TempData["msj"] = $"{hizmet.HizmetAd} başarıyla eklendi.";
                return RedirectToAction("Index");
            }
            return View(hizmet);
        }

        // Hizmet Düzenleme Sayfası (GET)
        [HttpGet]
        public IActionResult HizmetDuzenle(int? id)
        {
            if (id == null)
            {
                TempData["msj"] = "Geçersiz Hizmet ID.";
                return RedirectToAction("Index");
            }

            var hizmet = _context.Hizmetler.Find(id);
            if (hizmet == null)
            {
                TempData["msj"] = "Hizmet bulunamadı.";
                return RedirectToAction("Index");
            }

            return View(hizmet);
        }

        // Hizmet Düzenleme İşlemi (POST)
        [HttpPost]
        public IActionResult HizmetDuzenle(Hizmetler hizmet)
        {
            if (ModelState.IsValid)
            {
                _context.Hizmetler.Update(hizmet);
                _context.SaveChanges();
                TempData["msj"] = $"{hizmet.HizmetAd} başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            return View(hizmet);
        }

        // Hizmet Silme İşlemi
        [HttpPost]
        public IActionResult HizmetSil(int? id)
        {
            if (id == null)
            {
                TempData["msj"] = "Geçersiz Hizmet ID.";
                return RedirectToAction("Index");
            }

            var hizmet = _context.Hizmetler.Find(id);
            if (hizmet == null)
            {
                TempData["msj"] = "Hizmet bulunamadı.";
                return RedirectToAction("Index");
            }

            _context.Hizmetler.Remove(hizmet);
            _context.SaveChanges();
            TempData["msj"] = $"{hizmet.HizmetAd} başarıyla silindi.";
            return RedirectToAction("Index");
        }
    }
}
