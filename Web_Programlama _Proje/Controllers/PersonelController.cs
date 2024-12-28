//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using Web_Programlama__Proje.Models;

//namespace Web_Programlama__Proje.Controllers
//{
//    [Authorize(Roles = "Admin")]

//    public class PersonelController : Controller
//    {
//        RendevuContext _context=new RendevuContext();    

//        public IActionResult Index()
//        {
//            return View();
//        }

//        public IActionResult OnayVer()
//        {
//            return View();
//        }
//        public IActionResult PersonelKaydet(Personel personel)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Personaller.Add(personel);
//                _context.SaveChanges();
//                TempData["msj"] = personel.PersonelAd + " : Adlı Personel  kayıt edilmiştir";
//                return RedirectToAction("Personel");


//            }
//            TempData["msj"] = "Lütfen bilgilerin doğrulundan emin olun";
//            return RedirectToAction("personelEkle");
//        }
//        public IActionResult personelEkle()
//        {
//            // Hizmetleri liste olarak ViewBag'e gönder
//            ViewBag.HizmetListesi = new MultiSelectList(_context.Hizmetler, "HizmetID", "HizmetAd");
//            return View();
//        }

//        [HttpPost]
//        public IActionResult personelEkle(Personel personel, int[] hizmetler)
//        {
//            if (ModelState.IsValid)
//            {
//                // Personel ekle
//                _context.Personaller.Add(personel);
//                _context.SaveChanges();

//                // Seçilen hizmetleri personelle ilişkilendir
//                foreach (var hizmetID in hizmetler)
//                {
//                    _context.PersonelHizmetler.Add(new PersonelHizmet
//                    {
//                        PersonelID = personel.PersonelID,
//                        HizmetID = hizmetID
//                    });
//                }

//                _context.SaveChanges();
//                TempData["msj"] = "Personel başarıyla eklendi ve hizmetler atandı.";
//                return RedirectToAction("Personller");
//            }

//            // Hata durumunda yeniden hizmet listesi gönder
//            ViewBag.HizmetListesi = new MultiSelectList(_context.Hizmetler, "HizmetID", "HizmetAd");
//            return View(personel);
//        }

//        public IActionResult Personller()
//        {
//            var personeller = _context.Personaller
//                .Include(p => p.PersonelHizmetler)
//                .ThenInclude(ph => ph.Hizmetler)
//                .ToList();

//            foreach (var personel in personeller)
//            {
//                if (personel.PersonelHizmetler == null || !personel.PersonelHizmetler.Any())
//                {
//                    personel.PersonelHizmetler = new List<PersonelHizmet>();
//                }
//            }

//            return View(personeller);
//        }
//        public IActionResult PersonelSil(int? id)
//        {
//            if (id is null)
//            {
//                TempData["msj"] = "Lütfen düzgün giriniz";
//                return RedirectToAction("Personller");
//            }
//            var Personel = _context.Personaller.Find(id);
//            if (Personel is null)
//            {
//                TempData["msj"] = "bulunamadı  ";
//                return RedirectToAction("Personller");
//            }
//            //yukardakıyla aynı kod farklı şekilde yazılışı in join
//            var Kayit = _context.Personaller.Include(x => x.Rendevu).Where(x => x.PersonelID == id).ToList();
//            if (Kayit[0].Rendevu.Count > 0)
//            {
//                TempData["msj"] = "Yazara ait kitaplar var. Lütfen önce kitapları siliniz";
//                return RedirectToAction("Personller");
//            }
//            _context.Personaller.Remove(Personel);
//            _context.SaveChanges();
//            TempData["msj"] = Personel.PersonelAd + "adlı Personel silindi ";
//            return RedirectToAction("Personller");
//        }
//    }
//}
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

        // Personel Ekleme İşlemi (POST)
        [HttpPost]
        public IActionResult PersonelEkle(Personel personel, List<TimeSpan> calismaSaatleri, int[] hizmetler)
        {
            if (ModelState.IsValid)
            {
                // Personeli Kaydet
                _context.Personaller.Add(personel);
                _context.SaveChanges();

                // Çalışma Saatlerini Kaydet
                foreach (var calismaSaati in calismaSaatleri)
                {
                    var yeniCalismaSaati = new PersonelCalismaSaati
                    {
                        PersonelID = personel.PersonelID,
                        CalismaSaati = calismaSaati
                    };
                    _context.PersonelCalismaSaati.Add(yeniCalismaSaati);
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

    }
}
