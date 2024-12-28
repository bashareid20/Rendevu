
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web_Programlama__Proje.Models;

namespace Web_Programlama__Proje.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PersonelController : Controller
    {
        private RendevuContext _context = new RendevuContext();

        // Personelleri Listeleme (Personller)
        public IActionResult Personller()
        {
            var personeller = _context.Personaller
                .Include(p => p.PersonelHizmetler)
                .ThenInclude(ph => ph.Hizmetler)
                .Include(p => p.PersonelCalismaSaati)
                .ToList();

            return View(personeller);
        }

        // Personel Ekleme Sayfası (GET)
        public IActionResult PersonelEkle()
        {
            ViewBag.HizmetListesi = new MultiSelectList(_context.Hizmetler, "HizmetID", "HizmetAd");
            return View();
        }

        [HttpPost]
        public IActionResult PersonelEkle(Personel personel, List<DateTime> tarihler, List<TimeSpan> calismaSaatleri, int[] hizmetler)
        {
            if (ModelState.IsValid)
            {
                // Personel Kaydet
                _context.Personaller.Add(personel);
                _context.SaveChanges();

                // Tarih ve Çalışma Saatlerini Kaydet
                foreach (var tarih in tarihler)
                {
                    foreach (var saat in calismaSaatleri)
                    {
                        var yeniCalismaSaati = new PersonelCalismaSaati
                        {
                            PersonelID = personel.PersonelID,
                            CalismaSaati = saat,
                            Tarih = tarih
                        };
                        _context.PersonelCalismaSaati.Add(yeniCalismaSaati);
                    }
                }

                // Hizmetleri Kaydet
                foreach (var hizmetID in hizmetler)
                {
                    var personelHizmet = new PersonelHizmet
                    {
                        PersonelID = personel.PersonelID,
                        HizmetID = hizmetID
                    };
                    _context.PersonelHizmetler.Add(personelHizmet);
                }

                _context.SaveChanges();
                TempData["msj"] = $"{personel.PersonelAd} başarıyla kaydedildi.";
                return RedirectToAction("Personller");
            }

            ViewBag.HizmetListesi = new MultiSelectList(_context.Hizmetler, "HizmetID", "HizmetAd");
            TempData["msj"] = "Lütfen bilgileri kontrol ediniz.";
            return View(personel);
        }


        public IActionResult PersonelDetay(int? id)
        {
            if (!id.HasValue)
            {
                TempData["msj"] = "Geçersiz ID.";
                return RedirectToAction("Personller");
            }

            var personel = _context.Personaller
                .Include(p => p.PersonelHizmetler)
                .ThenInclude(ph => ph.Hizmetler)
                .Include(p => p.PersonelCalismaSaati)
                .FirstOrDefault(p => p.PersonelID == id);

            if (personel == null)
            {
                TempData["msj"] = "Personel bulunamadı.";
                return RedirectToAction("Personller");
            }

            return View(personel);
        }


        // Personel Silme
        public IActionResult PersonelSil(int? id)
        {
            if (!id.HasValue)
            {
                TempData["msj"] = "Geçersiz ID.";
                return RedirectToAction("Personller");
            }

            var personel = _context.Personaller
                .Include(p => p.PersonelCalismaSaati)
                .Include(p => p.PersonelHizmetler)
                .FirstOrDefault(p => p.PersonelID == id);

            if (personel == null)
            {
                TempData["msj"] = "Personel bulunamadı.";
                return RedirectToAction("Personller");
            }

            // Çalışma Saatlerini Sil
            var calismaSaatleri = _context.PersonelCalismaSaati
                .Where(cs => cs.PersonelID == personel.PersonelID)
                .ToList();
            _context.PersonelCalismaSaati.RemoveRange(calismaSaatleri);

            // Hizmetleri Sil
            var hizmetler = _context.PersonelHizmetler
                .Where(ph => ph.PersonelID == personel.PersonelID)
                .ToList();
            _context.PersonelHizmetler.RemoveRange(hizmetler);

            // Personeli Sil
            _context.Personaller.Remove(personel);
            _context.SaveChanges();

            TempData["msj"] = $"{personel.PersonelAd} başarıyla silindi.";
            return RedirectToAction("Personller");
        }
        public IActionResult PersonelDuzenle(int? id)
        {
            if (!id.HasValue)
            {
                TempData["msj"] = "Geçersiz ID.";
                return RedirectToAction("Personller");
            }

            var personel = _context.Personaller
                .Include(p => p.PersonelHizmetler)
                .ThenInclude(ph => ph.Hizmetler)
                .Include(p => p.PersonelCalismaSaati)
                .FirstOrDefault(p => p.PersonelID == id);

            if (personel == null)
            {
                TempData["msj"] = "Personel bulunamadı.";
                return RedirectToAction("Personller");
            }

            // Hizmetleri ViewBag ile gönder
            ViewBag.HizmetListesi = new MultiSelectList(_context.Hizmetler, "HizmetID", "HizmetAd");

            return View(personel);
        }
        [HttpPost]
        public IActionResult PersonelDuzenle(Personel personel, List<DateTime> tarihler, List<TimeSpan> calismaSaatleri, int[] hizmetler)
        {
            if (ModelState.IsValid)
            {
                // Personeli Güncelle
                var mevcutPersonel = _context.Personaller
                    .Include(p => p.PersonelHizmetler)
                    .Include(p => p.PersonelCalismaSaati)
                    .FirstOrDefault(p => p.PersonelID == personel.PersonelID);

                if (mevcutPersonel == null)
                {
                    TempData["msj"] = "Personel bulunamadı.";
                    return RedirectToAction("Personller");
                }

                mevcutPersonel.PersonelAd = personel.PersonelAd;
                mevcutPersonel.PersonelSoyAd = personel.PersonelSoyAd;
                mevcutPersonel.PersonelYetenekleri = personel.PersonelYetenekleri;

                // Çalışma Saatlerini Güncelle
                _context.PersonelCalismaSaati.RemoveRange(mevcutPersonel.PersonelCalismaSaati);
                foreach (var tarih in tarihler)
                {
                    foreach (var saat in calismaSaatleri)
                    {
                        var yeniCalismaSaati = new PersonelCalismaSaati
                        {
                            PersonelID = mevcutPersonel.PersonelID,
                            Tarih = tarih,
                            CalismaSaati = saat
                        };
                        _context.PersonelCalismaSaati.Add(yeniCalismaSaati);
                    }
                }

                // Hizmetleri Güncelle
                _context.PersonelHizmetler.RemoveRange(mevcutPersonel.PersonelHizmetler);
                foreach (var hizmetID in hizmetler)
                {
                    var personelHizmet = new PersonelHizmet
                    {
                        PersonelID = mevcutPersonel.PersonelID,
                        HizmetID = hizmetID
                    };
                    _context.PersonelHizmetler.Add(personelHizmet);
                }

                _context.SaveChanges();
                TempData["msj"] = $"{mevcutPersonel.PersonelAd} başarıyla güncellendi.";
                return RedirectToAction("Personller");
            }

            // Hata durumunda hizmet listesini yeniden gönder
            ViewBag.HizmetListesi = new MultiSelectList(_context.Hizmetler, "HizmetID", "HizmetAd");
            TempData["msj"] = "Lütfen bilgileri kontrol ediniz.";
            return View(personel);
        }

    }
}
