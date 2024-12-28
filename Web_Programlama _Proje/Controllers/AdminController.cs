//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System;
//using Web_Programlama__Proje.Areas.Identity.Data;
//using Web_Programlama__Proje.Models;
//[Authorize(Roles ="Admin")]
//public class AdminController : Controller
//{
//   RendevuContext _context =new RendevuContext();

//    // GET: Admin Dashboard





//        // Admin Paneli Ana Sayfa
//        public IActionResult Index()
//        {
//            var bekleyenRandevular = _context.Rendevular
//                .Include(r => r.Personel)
//                .Where(r => !r.RendevuOnayDurumu)
//                .ToList();

//            ViewBag.BekleyenRandevuSayisi = bekleyenRandevular.Count();
//            return View(bekleyenRandevular);
//        }

//        // Randevu Onaylama
//        [HttpPost]
//        public IActionResult RandevuOnayla(int id)
//        {
//            var randevu = _context.Rendevular.Find(id);
//            if (randevu == null)
//            {
//                TempData["msj"] = "Randevu bulunamadı.";
//                return RedirectToAction("Index");
//            }

//            randevu.RendevuOnayDurumu = true; // Onaylanmış
//            _context.SaveChanges();
//            TempData["msj"] = "Randevu onaylandı.";
//            return RedirectToAction("Index");
//        }

//        // Randevu Silme
//        [HttpPost]
//        public IActionResult RandevuSil(int id)
//        {
//            var randevu = _context.Rendevular.Find(id);
//            if (randevu == null)
//            {
//                TempData["msj"] = "Randevu bulunamadı.";
//                return RedirectToAction("Index");
//            }

//            _context.Rendevular.Remove(randevu);
//            _context.SaveChanges();
//            TempData["msj"] = "Randevu silindi.";
//            return RedirectToAction("Index");
//        }

//}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Programlama__Proje.Areas.Identity.Data;
using Web_Programlama__Proje.Models;

public class AdminController : Controller
{
   RendevuContext _context =new RendevuContext();

    // Admin Paneli Ana Sayfa
    public IActionResult Index()
    {
        var bekleyenRandevular = _context.Rendevular
            .Include(r => r.Personel)
            .Where(r => r.RendevuDurumu == null) // Sadece bekleyen randevuları getir
            .ToList();

        ViewBag.BekleyenRandevuSayisi = bekleyenRandevular.Count();
        return View(bekleyenRandevular);
    }

    // Randevu Onaylama
    [HttpPost]
    public IActionResult RandevuOnayla(int id)
    {
        var randevu = _context.Rendevular.Find(id);
        if (randevu == null)
        {
            TempData["msj"] = "Randevu bulunamadı.";
            return RedirectToAction("Index");
        }

        randevu.RendevuDurumu = 1; // Onaylandı
        _context.SaveChanges();
        TempData["msj"] = "Randevu onaylandı.";
        return RedirectToAction("Index");
    }

    // Randevu Reddetme
    [HttpPost]
    public IActionResult RandevuReddet(int id)
    {
        var randevu = _context.Rendevular.Find(id);
        if (randevu == null)
        {
            TempData["msj"] = "Randevu bulunamadı.";
            return RedirectToAction("Index");
        }

        randevu.RendevuDurumu = 0; // Reddedildi
        _context.SaveChanges();
        TempData["msj"] = "Randevu reddedildi.";
        return RedirectToAction("Index");
    }
}

