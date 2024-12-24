using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using Web_Programlama__Proje.Models;
namespace Web_Programlama__Proje.Controllers
{
    [Authorize]
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

        //public IActionResult RendevuKaydet(Rendevu rendevu)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        foreach (var hizmetId in rendevu.Hizmetler)
        //        {
        //            var hizmet = _context.Hizmetler.Find(hizmetId);
        //            // Seçilen hizmetlerle ilgili işlemleri yapabilirsiniz
        //        }

        //        // Aynı personel ve saat için randevu kontrolü
        //        bool cakisiyorMu = _context.Rendevular
        //            .Any(r => r.PersonelID == rendevu.PersonelID && r.RendevuZaman == rendevu.RendevuZaman);

        //        if (cakisiyorMu)
        //        {
        //            // Çakışma durumunda kullanıcıya mesaj döndürülür
        //            TempData["msj"] = "Seçtiğiniz personel için aynı saat diliminde zaten bir randevu mevcut!";
        //            return RedirectToAction("RendevuAl");
        //        }

        //        // Eğer çakışma yoksa randevu kaydedilir
        //        _context.Rendevular.Add(rendevu);
        //        _context.SaveChanges();
        //        TempData["msj"] = $"{rendevu.MusteriAd} için randevu başarıyla kaydedildi.";
        //        return RedirectToAction("Rendevular");
        //    }

        //    TempData["msj"] = "Lütfen bilgilerin doğruluğundan emin olun!";
        //    return RedirectToAction("RendevuAl");
        //}
        private int GetToplamHizmetSuresi(int musteriiId)
{
    // İlgili müşteri randevusuna ait hizmet sürelerini al
    var hizmetSuresi = _context.PersonelHizmetler
        .Where(ph => _context.Rendevular
            .Any(r => r.MusteriiID == musteriiId && r.PersonelID == ph.PersonelID))
        .Select(ph => ph.Hizmetler.HizmetSuresi)
        .ToList();

            // Toplam süreyi hesapla
            return hizmetSuresi.Sum();

        }

        public IActionResult RendevuKaydet(Rendevu rendevu)
        {
            if (ModelState.IsValid)
            {
                // Hizmetlerin toplam süresini hesapla
                int toplamHizmetSuresi = 0;
                foreach (var hizmetId in rendevu.Hizmetler)
                {
                    var hizmet = _context.Hizmetler.Find(hizmetId);
                    if (hizmet != null)
                    {
                        toplamHizmetSuresi += hizmet.HizmetSuresi; // Hizmet süresinin int olduğunu varsaydık
                    }
                }

                // Randevu bitiş zamanını hesapla
                DateTime randevuBaslangic = rendevu.RendevuZaman;
                DateTime randevuBitis = randevuBaslangic.AddMinutes(toplamHizmetSuresi);

                // Aynı personel ve zaman aralığında başka randevu var mı kontrol et
                // Veritabanından ilgili randevuları ve personelleri liste olarak alın
                var randevular = _context.Rendevular
                    .Where(r => r.PersonelID == rendevu.PersonelID)
                    .ToList(); // Verileri belleğe al

                bool cakisiyorMu = randevular.Any(r =>
                {
                    // Mevcut randevular için hizmet sürelerini hesapla
                    int mevcutRandevuToplamSuresi = GetToplamHizmetSuresi(r.MusteriiID);

                    // Randevu başlangıç ve bitiş zamanlarını hesapla
                    DateTime mevcutRandevuBaslangic = r.RendevuZaman;
                    DateTime mevcutRandevuBitis = mevcutRandevuBaslangic.AddMinutes(mevcutRandevuToplamSuresi);

                    // Yeni randevu başlangıç ve bitiş zamanları
                    DateTime yeniRandevuBaslangic = rendevu.RendevuZaman;
                    DateTime yeniRandevuBitis = yeniRandevuBaslangic.AddMinutes(toplamHizmetSuresi);

                    // Çakışma kontrolü
                    return (mevcutRandevuBaslangic < yeniRandevuBitis && mevcutRandevuBitis > yeniRandevuBaslangic);
                });

                if (cakisiyorMu)
                {
                    TempData["msj"] = "Seçtiğiniz personel için bu zaman diliminde çakışan bir randevu mevcut!";
                    return RedirectToAction("RendevuAl");
                }


                // Eğer çakışma yoksa randevu kaydedilir
                _context.Rendevular.Add(rendevu);
                _context.SaveChanges();
                TempData["msj"] = $"{rendevu.MusteriAd} için randevu başarıyla kaydedildi.";
                return RedirectToAction("Rendevular");
            }

            TempData["msj"] = "Lütfen bilgilerin doğruluğundan emin olun!";
            return RedirectToAction("RendevuAl");
        }


        public IActionResult RendavuDetay(Rendevu r) 
        {
            return View(r);
        }
        public IActionResult RendavuSil(int? id) 
        {
            if (id is null)
            {
                TempData["msj"] = "Lütfen düzgün giriniz";
                return RedirectToAction("Rendevular");
            }
            var yazar = _context.Rendevular.Find(id);
            if (yazar is null)
            {
                TempData["msj"] = "bulunamadı  ";
                return RedirectToAction("Rendevular");
            }
            //yukardakıyla aynı kod farklı şekilde yazılışı in join
          
            _context.Rendevular.Remove(yazar);
            _context.SaveChanges();
            TempData["msj"] = yazar.MusteriAd + "adlı Müşterinin Rendevusu silindi ";
            return RedirectToAction("Rendevular");

        }

        public IActionResult RendevuAl()
        {
            var personeller = _context.Personaller
        .Select(p => new { p.PersonelID, FullName = p.PersonelAd + " " + p.PersonelSoyAd }).ToList();
            ViewBag.PersonelListesi = new SelectList(personeller, "PersonelID", "FullName");

            var hizmetler = _context.Hizmetler.ToList();
            ViewBag.HizmetListesi = hizmetler;

            return View();
        }
    }
}
