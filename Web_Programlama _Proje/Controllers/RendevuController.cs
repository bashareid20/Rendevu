using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
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
        //        // Müşteri var mı kontrol et (bugün veya gelecekte randevusu var mı?)
        //        var mevcutRandevular = _context.Rendevular
        //            .Where(m => m.MusteriMail == rendevu.MusteriMail || m.MusteriTelefonNo == rendevu.MusteriTelefonNo)
        //            .ToList();

        //        if (mevcutRandevular.Any(r => r.RendevuZaman.Date >= DateTime.Now.Date))
        //        {
        //            // Eğer müşteri bugün veya daha sonraki bir tarihte randevu almışsa, uyarı ver
        //            TempData["msj_sorgula"] = "Bu e-posta veya telefon numarasıyla bugüne veya geleceğe ait bir randevu zaten alınmış!";
        //            return RedirectToAction("RendevuSorgula"); // Uygun sayfaya yönlendirme
        //        }


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
        //        var randevular = _context.Rendevular
        //            .Where(r => r.PersonelID == rendevu.PersonelID)
        //            .ToList();

        //        bool cakisiyorMu = randevular.Any(r =>
        //        {
        //            int mevcutRandevuToplamSuresi = GetToplamHizmetSuresi(r.MusteriiID);
        //            DateTime mevcutRandevuBaslangic = r.RendevuZaman;
        //            DateTime mevcutRandevuBitis = mevcutRandevuBaslangic.AddMinutes(mevcutRandevuToplamSuresi);

        //            return (mevcutRandevuBaslangic < randevuBitis && mevcutRandevuBitis > randevuBaslangic);
        //        });

        //        if (cakisiyorMu)
        //        {
        //            TempData["msj"] = "Seçtiğiniz personel için bu zaman diliminde çakışan bir randevu mevcut!";
        //            return RedirectToAction("RendevuAl");
        //        }

        //        // Randevu kaydet
        //        _context.Rendevular.Add(rendevu);
        //        _context.SaveChanges();
        //        TempData["msj"] = $"{rendevu.MusteriAd} için randevu başarıyla kaydedildi.";
        //        return RedirectToAction("Rendevular");
        //    }

        //    TempData["msj"] = "Lütfen bilgilerin doğruluğundan emin olun!";
        //    return RedirectToAction("RendevuAl");
        //}

        //[HttpPost]
        //public IActionResult RandevuKaydet(Rendevu rendevu)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Randevuyu kaydet
        //        _context.Rendevular.Add(rendevu);
        //        _context.SaveChanges();

        //        // Seçilen saat ve çakışan saatleri sil
        //        if (rendevu.RendevuZaman != null && rendevu.PersonelID != 0)
        //        {
        //            var calismaSaati = _context.PersonelCalismaSaati
        //                .Where(cs => cs.PersonelID == rendevu.PersonelID && cs.CalismaSaati == rendevu.RendevuZaman.TimeOfDay)
        //                .FirstOrDefault();

        //            if (calismaSaati != null)
        //            {
        //                _context.PersonelCalismaSaati.Remove(calismaSaati);
        //                _context.SaveChanges();
        //            }
        //        }

        //        TempData["msj"] = "Randevu başarıyla kaydedildi.";
        //        return RedirectToAction("Index");
        //    }

        //    TempData["msj"] = "Lütfen tüm alanları doldurunuz.";
        //    return RedirectToAction("RendevuAl");
        //}
        [HttpPost]
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
                    TempData["msj_sorgula"] = "Bu e-posta veya telefon numarasıyla bugüne veya geleceğe ait bir randevu zaten alınmış!";
                    return RedirectToAction("RendevuSorgula");
                }

                // Hizmetlerin toplam süresini hesapla
                int toplamHizmetSuresi = 0;
                foreach (var hizmetId in rendevu.Hizmetler)
                {
                    var hizmet = _context.Hizmetler.Find(hizmetId);
                    if (hizmet != null)
                    {
                        toplamHizmetSuresi += hizmet.HizmetSuresi;
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

                // Seçilen saat ve çakışan saatleri personel çalışma saatlerinden sil
                if (rendevu.RendevuZaman != null && rendevu.PersonelID != 0)
                {
                    var calismaSaatleri = _context.PersonelCalismaSaati
                        .Where(cs => cs.PersonelID == rendevu.PersonelID
                                     && cs.CalismaSaati >= rendevu.RendevuZaman.TimeOfDay
                                     && cs.CalismaSaati < rendevu.RendevuZaman.TimeOfDay.Add(TimeSpan.FromMinutes(toplamHizmetSuresi)))
                        .ToList();

                    if (calismaSaatleri.Any())
                    {
                        _context.PersonelCalismaSaati.RemoveRange(calismaSaatleri);
                        _context.SaveChanges();
                    }
                }

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

        //public IActionResult RendevuAl()
        //{
        //    var personeller = _context.Personaller
        //.Select(p => new { p.PersonelID, FullName = p.PersonelAd + " " + p.PersonelSoyAd }).ToList();
        //    ViewBag.PersonelListesi = new SelectList(personeller, "PersonelID", "FullName");

        //    var hizmetler = _context.Hizmetler.ToList();
        //    ViewBag.HizmetListesi = hizmetler;

        //    return View();
        //}
        public IActionResult RendevuAl()
        {
            var personeller = _context.Personaller
                .Select(p => new { p.PersonelID, FullName = p.PersonelAd + " " + p.PersonelSoyAd })
                .ToList();

            ViewBag.PersonelListesi = new SelectList(personeller, "PersonelID", "FullName");
            ViewBag.HizmetListesi = _context.Hizmetler.ToList();
            return View();
        }


        [HttpPost]
        public IActionResult GetAvailableHoursInView(int personelID, DateTime tarih)
        {
            var calismaSaatleri = _context.PersonelCalismaSaati
                .Where(cs => cs.PersonelID == personelID && cs.Tarih.Date == tarih.Date)
                .Select(cs => cs.CalismaSaati)
                .ToList();

            System.Diagnostics.Debug.WriteLine($"Çalışma Saatleri: {string.Join(", ", calismaSaatleri)}");

            return PartialView("_UygunSaatler", calismaSaatleri);
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
        [HttpPost]
        public JsonResult DeleteHour([FromBody] DeleteHourRequest request)
        {
            try
            {
                // Verilen tarih ve saati birleştirerek karşılaştırma yap
                DateTime combinedDateTime = DateTime.Parse($"{request.Tarih} {request.Hour}");

                // Çalışma saati tablosundan uygun kaydı bul
                var calismaSaati = _context.PersonelCalismaSaati
                    .FirstOrDefault(cs => cs.PersonelID == request.PersonelID
                                          && cs.Tarih.Date == combinedDateTime.Date
                                          && cs.CalismaSaati == combinedDateTime.TimeOfDay);

                if (calismaSaati != null)
                {
                    _context.PersonelCalismaSaati.Remove(calismaSaati); // Saat kaydını sil
                    _context.SaveChanges(); // Değişiklikleri kaydet
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "Çalışma saati bulunamadı." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Request için kullanılan DTO sınıfı
        public class DeleteHourRequest
        {
            public int PersonelID { get; set; }
            public string Tarih { get; set; }
            public string Hour { get; set; }
        }


        //[HttpGet]
        //public JsonResult GetAvailableHours(int personelID, DateTime date)
        //{
        //    // Personel ve çalışma saatleri alınır
        //    var personel = _context.Personaller
        //        .Include(p => p.PersonelCalismaSaati)
        //        .FirstOrDefault(p => p.PersonelID == personelID);

        //    if (personel == null || personel.PersonelCalismaSaati == null)
        //    {
        //        return Json(new { success = false, message = "Personel veya çalışma saatleri bulunamadı." });
        //    }

        //    // Çalışma saatlerini al
        //    var calismaSaatleri = personel.PersonelCalismaSaati.Select(cs => cs.CalismaSaati).ToList();

        //    // Mevcut randevuların saatlerini kontrol et
        //    var mevcutRandevular = _context.Rendevular
        //        .Where(r => r.PersonelID == personelID && r.RendevuZaman.Date == date.Date)
        //        .Select(r => r.RendevuZaman.TimeOfDay)
        //        .ToList();

        //    // Uygun saatleri filtrele
        //    var uygunSaatler = calismaSaatleri.Where(cs => !mevcutRandevular.Contains(cs)).ToList();

        //    return Json(new { success = true, hours = uygunSaatler });
        //}
        //[HttpGet]
        //public JsonResult GetAvailableHours(int personelID, DateTime date)
        //{
        //    var personel = _context.Personaller
        //        .Include(p => p.PersonelCalismaSaati)
        //        .FirstOrDefault(p => p.PersonelID == personelID);

        //    if (personel == null || personel.PersonelCalismaSaati == null)
        //        return Json(new { success = false, message = "Personel bilgisi bulunamadı." });

        //    var calismaSaatleri = personel.PersonelCalismaSaati.Select(cs => cs.CalismaSaati).ToList();

        //    // Mevcut randevuları al
        //    var mevcutRandevular = _context.Rendevular
        //        .Where(r => r.PersonelID == personelID && r.RendevuZaman.Date == date.Date)
        //        .ToList()
        //        .Select(r =>
        //        {
        //            var toplamSuresi = r.Hizmetler
        //                .Select(hizmetID => _context.Hizmetler.FirstOrDefault(h => h.HizmetID == hizmetID)?.HizmetSuresi ?? 0)
        //                .Sum();

        //            return new
        //            {
        //                Baslangic = r.RendevuZaman.TimeOfDay,
        //                Bitis = r.RendevuZaman.AddMinutes(toplamSuresi).TimeOfDay
        //            };
        //        })
        //        .ToList();

        //    // Uygun saatleri filtrele
        //    var uygunSaatler = calismaSaatleri
        //        .Where(cs => !mevcutRandevular.Any(mr => cs >= mr.Baslangic && cs < mr.Bitis))
        //        .ToList();

        //    if (!uygunSaatler.Any())
        //        return Json(new { success = false, message = "Uygun saat yok." });

        //    var formattedHours = uygunSaatler.Select(cs => cs.ToString(@"hh\:mm")).ToList();
        //    return Json(new { success = true, hours = formattedHours });
        //}
        //[HttpGet]
        //public JsonResult GetAvailableHours(int personelID, DateTime date)
        //{
        //    // Personelin o gün çalıştığı saatleri al
        //    var calismaSaatleri = _context.PersonelCalismaSaati
        //        .Where(cs => cs.PersonelID == personelID && cs.Tarih.Date == date.Date)
        //        .Select(cs => cs.CalismaSaati)
        //        .OrderBy(cs => cs)
        //        .ToList();

        //    if (!calismaSaatleri.Any())
        //    {
        //        return Json(new { success = false, message = "Çalışma saatleri bulunamadı." });
        //    }

        //    // Mevcut randevuları ve çakışma sürelerini kontrol et
        //    var mevcutRandevular = _context.Rendevular
        //        .Where(r => r.PersonelID == personelID && r.RendevuZaman.Date == date.Date)
        //        .Include(r => r.RendevuHizmetler) // Ara tabloyu dahil et
        //        .ThenInclude(rh => rh.Hizmetler) // Hizmetler tablosunu dahil et
        //        .Select(r => new
        //        {
        //            Baslangic = r.RendevuZaman.TimeOfDay,
        //            Bitis = r.RendevuZaman.TimeOfDay.Add(TimeSpan.FromMinutes(
        //                r.RendevuHizmetler.Sum(rh => rh.Hizmetler.HizmetSuresi) // Hizmet sürelerini topla
        //            ))
        //        })
        //        .ToList();

        //    // Çakışma kontrolü
        //    var uygunSaatler = calismaSaatleri
        //        .Where(cs => !mevcutRandevular.Any(r =>
        //            cs >= r.Baslangic && cs < r.Bitis // Çakışma kontrolü
        //        ))
        //        .OrderBy(cs => cs)
        //        .ToList();

        //    // Saatleri döndür
        //    if (!uygunSaatler.Any())
        //    {
        //        return Json(new { success = false, message = "Uygun saat bulunamadı." });
        //    }

        //    var formattedHours = uygunSaatler.Select(cs => cs.ToString(@"hh\:mm")).ToList();
        //    return Json(new { success = true, hours = formattedHours });
        //}
        [HttpGet]
        public JsonResult GetAvailableHours(int personelID, DateTime tarih)
        {
            // Çalışma saatlerini al
            var calismaSaatleri = _context.PersonelCalismaSaati
                .Where(cs => cs.PersonelID == personelID && cs.Tarih.Date == tarih.Date)
                .Select(cs => cs.CalismaSaati)
                .ToList();

            // Mevcut randevuları al
            var mevcutRandevular = _context.Rendevular
                .Where(r => r.PersonelID == personelID && r.RendevuZaman.Date == tarih.Date)
                .Select(r => new
                {
                    Baslangic = r.RendevuZaman.TimeOfDay,
                    Bitis = r.RendevuZaman.TimeOfDay.Add(TimeSpan.FromMinutes(
                        r.RendevuHizmetler.Sum(rh => rh.Hizmetler.HizmetSuresi)
                    ))
                })
                .ToList();

            // Uygun saatleri hesapla
            var uygunSaatler = calismaSaatleri
                .Where(cs => !mevcutRandevular.Any(r =>
                    cs >= r.Baslangic && cs < r.Bitis
                ))
                .ToList();

            // Eğer çalışma saatleri varsa, bunları döndür
            if (uygunSaatler.Any())
            {
                return Json(new { success = true, uygunSaatler = uygunSaatler.Select(cs => cs.ToString(@"hh\:mm")).ToList() });
            }

            return Json(new { success = false, message = "Bu tarihte uygun saat bulunamadı." });
        }


        public IActionResult RendevuSorgula()
        {
            return View();
        }
        public IActionResult RendevuSonuc()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RendevuSorgulaSonuc(string MusteriEmail, string MusteriTelefonNo)
        {
            var randevular = _context.Rendevular
                .Include(r => r.Personel)
                .Where(r => r.MusteriMail == MusteriEmail && r.MusteriTelefonNo == MusteriTelefonNo)
                .ToList();

            if (!randevular.Any())
            {
                TempData["msj_sorgula"] = "Girilen bilgilere ait bir randevu bulunamadı.";
                return RedirectToAction("RendevuSorgula");
            }

            ViewBag.HizmetListesi = _context.Hizmetler.ToList();
            return View("RendevuSonuc", randevular);
        }

    }
}
