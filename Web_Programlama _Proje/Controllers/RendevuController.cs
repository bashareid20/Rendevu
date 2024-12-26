using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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


        //public IActionResult RendevuKaydet(Rendevu rendevu)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Hizmetlerin toplam süresini hesapla
        //        int toplamHizmetSuresi = 0;
        //        foreach (var hizmetId in rendevu.Hizmetler)
        //        {
        //            var hizmet = _context.Hizmetler.Find(hizmetId);
        //            if (hizmet != null)
        //            {
        //                toplamHizmetSuresi += hizmet.HizmetSuresi; // Hizmet süresinin int olduğunu varsaydık
        //            }
        //        }

        //        // Randevu bitiş zamanını hesapla
        //        DateTime randevuBaslangic = rendevu.RendevuZaman;
        //        DateTime randevuBitis = randevuBaslangic.AddMinutes(toplamHizmetSuresi);

        //        // Aynı personel ve zaman aralığında başka randevu var mı kontrol et
        //        // Veritabanından ilgili randevuları ve personelleri liste olarak alın
        //        var randevular = _context.Rendevular
        //            .Where(r => r.PersonelID == rendevu.PersonelID)
        //            .ToList(); // Verileri belleğe al

        //        bool cakisiyorMu = randevular.Any(r =>
        //        {
        //            // Mevcut randevular için hizmet sürelerini hesapla
        //            int mevcutRandevuToplamSuresi = GetToplamHizmetSuresi(r.MusteriiID);

        //            // Randevu başlangıç ve bitiş zamanlarını hesapla
        //            DateTime mevcutRandevuBaslangic = r.RendevuZaman;
        //            DateTime mevcutRandevuBitis = mevcutRandevuBaslangic.AddMinutes(mevcutRandevuToplamSuresi);

        //            // Yeni randevu başlangıç ve bitiş zamanları
        //            DateTime yeniRandevuBaslangic = rendevu.RendevuZaman;
        //            DateTime yeniRandevuBitis = yeniRandevuBaslangic.AddMinutes(toplamHizmetSuresi);

        //            // Çakışma kontrolü
        //            return (mevcutRandevuBaslangic < yeniRandevuBitis && mevcutRandevuBitis > yeniRandevuBaslangic);
        //        });

        //        if (cakisiyorMu)
        //        {
        //            TempData["msj"] = "Seçtiğiniz personel için bu zaman diliminde çakışan bir randevu mevcut!";
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
        public IActionResult RendevuKaydet(Rendevu rendevu)
        {
            if (ModelState.IsValid)
            {
                // Müşteri var mı kontrol et (bugün veya gelecekte randevusu var mı?)
                var mevcutRandevular = _context.Rendevular
                    .Where(m => m.MusteriMail == rendevu.MusteriMail || m.MusteriTelefonNo == rendevu.MusteriTelefonNo)
                    .ToList();

                if (mevcutRandevular.Any(r => r.RendevuZaman.Date >= DateTime.Now.Date))
                {
                    // Eğer müşteri bugün veya daha sonraki bir tarihte randevu almışsa, uyarı ver
                    TempData["msj_sorgula"] = "Bu e-posta veya telefon numarasıyla bugüne veya geleceğe ait bir randevu zaten alınmış!";
                    return RedirectToAction("RendevuSorgula"); // Uygun sayfaya yönlendirme
                }


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
                var randevular = _context.Rendevular
                    .Where(r => r.PersonelID == rendevu.PersonelID)
                    .ToList();

                bool cakisiyorMu = randevular.Any(r =>
                {
                    int mevcutRandevuToplamSuresi = GetToplamHizmetSuresi(r.MusteriiID);
                    DateTime mevcutRandevuBaslangic = r.RendevuZaman;
                    DateTime mevcutRandevuBitis = mevcutRandevuBaslangic.AddMinutes(mevcutRandevuToplamSuresi);

                    return (mevcutRandevuBaslangic < randevuBitis && mevcutRandevuBitis > randevuBaslangic);
                });

                if (cakisiyorMu)
                {
                    TempData["msj"] = "Seçtiğiniz personel için bu zaman diliminde çakışan bir randevu mevcut!";
                    return RedirectToAction("RendevuAl");
                }

                // Randevu kaydet
                _context.Rendevular.Add(rendevu);
                _context.SaveChanges();
                TempData["msj"] = $"{rendevu.MusteriAd} için randevu başarıyla kaydedildi.";
                return RedirectToAction("Rendevular");
            }

            TempData["msj"] = "Lütfen bilgilerin doğruluğundan emin olun!";
            return RedirectToAction("RendevuAl");
        }



        public IActionResult RendavuDetay(int? id)
        {
            if (id is null)
            {
                TempData["msj"] = "Lütfen düzgün giriniz";
                return RedirectToAction("Rendevular");
            }

            var rendevu = _context.Rendevular
                .Include(x => x.Personel)
                .ThenInclude(p => p.PersonelHizmetler)
                .ThenInclude(ph => ph.Hizmetler)
                .FirstOrDefault(x => x.MusteriiID == id);

            if (rendevu is null)
            {
                TempData["msj"] = "Randevu bulunamadı.";
                return RedirectToAction("Index");
            }

            // Toplam hizmet süresi ve ödenecek miktar hesaplama
            var toplamHizmetSuresi = rendevu.Personel.PersonelHizmetler.Sum(ph => ph.Hizmetler.HizmetSuresi);
            var toplamHizmetUcreti = rendevu.Personel.PersonelHizmetler.Sum(ph => ph.Hizmetler.HizmetUcreti);

            ViewBag.ToplamHizmetSuresi = toplamHizmetSuresi;
            ViewBag.ToplamHizmetUcreti = toplamHizmetUcreti;

            return View(rendevu);
        }
      

        [HttpGet]
public IActionResult RendevuDuzenle(int? id)
{
    if (id is null)
    {
        TempData["msj"] = "Lütfen düzgün bir ID giriniz.";
        return RedirectToAction("Rendevular");
    }

    var rendevu = _context.Rendevular
        .Include(r => r.Personel)
        .FirstOrDefault(r => r.MusteriiID == id);

    if (rendevu == null)
    {
        TempData["msj"] = "Randevu bulunamadı.";
        return RedirectToAction("Rendevular");
    }

    // Mevcut seçili hizmetleri `Hizmetler` listesine doldur
    rendevu.Hizmetler = _context.PersonelHizmetler
        .Where(ph => ph.PersonelID == rendevu.PersonelID)
        .Select(ph => ph.HizmetID)
        .ToList();

    // Hizmet Listesi
    var hizmetler = _context.Hizmetler.ToList();
    ViewBag.HizmetListesi = hizmetler.Select(h => new SelectListItem
    {
        Value = h.HizmetID.ToString(),
        Text = h.HizmetAd,
        Selected = rendevu.Hizmetler.Contains(h.HizmetID) // Mevcut hizmetler işaretli
    });

    // Personel Listesi
    var personeller = _context.Personaller
        .Select(p => new { p.PersonelID, FullName = p.PersonelAd + " " + p.PersonelSoyAd })
        .ToList();
    ViewBag.PersonelListesi = new SelectList(personeller, "PersonelID", "FullName");

    return View(rendevu);
}

        [HttpPost]
        public IActionResult RendevuDuzenle(int? id, Rendevu updatedRendevu)
        {
            if (id is null)
            {
                TempData["msj"] = "Lütfen düzgün bir ID giriniz.";
                return RedirectToAction("Rendevular");
            }

            if (id != updatedRendevu.MusteriiID)
            {
                TempData["msj"] = "ID'ler eşleşmiyor.";
                return RedirectToAction("Rendevular");
            }

            var existingRendevu = _context.Rendevular
                .Include(r => r.Personel)
                .FirstOrDefault(r => r.MusteriiID == id);

            if (existingRendevu == null)
            {
                TempData["msj"] = "Randevu bulunamadı.";
                return RedirectToAction("Rendevular");
            }

            // Eski alanları koru
            updatedRendevu.MusteriMail = updatedRendevu.MusteriMail ?? existingRendevu.MusteriMail;
            updatedRendevu.MusteriTelefonNo = updatedRendevu.MusteriTelefonNo ?? existingRendevu.MusteriTelefonNo;

            // Personel hizmetlerini güncelle
            var mevcutHizmetler = _context.PersonelHizmetler
                .Where(ph => ph.PersonelID == existingRendevu.PersonelID)
                .ToList();

            // Eski hizmetleri sil
            _context.PersonelHizmetler.RemoveRange(mevcutHizmetler);

            // Yeni hizmetleri ekle
            if (updatedRendevu.Hizmetler != null && updatedRendevu.Hizmetler.Any())
            {
                foreach (var hizmetId in updatedRendevu.Hizmetler)
                {
                    _context.PersonelHizmetler.Add(new PersonelHizmet
                    {
                        PersonelID = existingRendevu.PersonelID,
                        HizmetID = hizmetId
                    });
                }
            }

            // Diğer alanları güncelle
            existingRendevu.MusteriAd = updatedRendevu.MusteriAd;
            existingRendevu.MusteriSoyAd = updatedRendevu.MusteriSoyAd;
            existingRendevu.MusteriTelefonNo = updatedRendevu.MusteriTelefonNo ?? existingRendevu.MusteriTelefonNo;
            existingRendevu.MusteriMail = updatedRendevu.MusteriMail ?? existingRendevu.MusteriMail;
            existingRendevu.RendevuZaman = updatedRendevu.RendevuZaman;
            existingRendevu.PersonelID = updatedRendevu.PersonelID;

            _context.SaveChanges();
            TempData["msj"] = "Randevu ve hizmetler başarıyla güncellendi.";
            return RedirectToAction("Rendevular");
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
        [HttpGet]
        public JsonResult CheckAvailability(int personelID, DateTime randevuZaman, int toplamHizmetSuresi)
        {
            // Personelin mevcut randevularını alın
            var randevular = _context.Rendevular
                .Where(r => r.PersonelID == personelID)
                .ToList();

            // Yeni randevunun başlangıç ve bitiş zamanlarını hesaplayın
            DateTime yeniRandevuBaslangic = randevuZaman;
            DateTime yeniRandevuBitis = yeniRandevuBaslangic.AddMinutes(toplamHizmetSuresi);

            // Çakışma kontrolü
            bool cakisiyorMu = randevular.Any(r =>
            {
                // Mevcut randevunun başlangıç ve bitiş zamanlarını hesaplayın
                int mevcutHizmetSuresi = GetToplamHizmetSuresi(r.MusteriiID);
                DateTime mevcutRandevuBaslangic = r.RendevuZaman;
                DateTime mevcutRandevuBitis = mevcutRandevuBaslangic.AddMinutes(mevcutHizmetSuresi);

                // Çakışma varsa true döndür
                return (mevcutRandevuBaslangic < yeniRandevuBitis && mevcutRandevuBitis > yeniRandevuBaslangic);
            });

            return Json(new { isAvailable = !cakisiyorMu });
        }


    }
}
