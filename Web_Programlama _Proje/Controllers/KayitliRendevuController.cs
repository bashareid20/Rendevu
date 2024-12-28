using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_Programlama__Proje.Areas.Identity.Data;
using Web_Programlama__Proje.Models;

namespace Web_Programlama__Proje.Controllers
{
    [Authorize]
    public class KayitliRendevuController : Controller
    {
        private readonly UserManager<DetilsUser> _userManager;
        RendevuContext _context =new RendevuContext();

        public KayitliRendevuController(UserManager<DetilsUser> userManager)
        {
            _userManager = userManager;
          
        }


        [HttpGet]
        public async Task<IActionResult> RandevuAl()
        {
            // Giriş yapan kullanıcı bilgilerini al
            var email = User.Identity.Name; // Kullanıcının giriş yaptığı e-posta
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                TempData["msj_kaytli"] = "Kullanıcı bilgileri bulunamadı.";
                return RedirectToAction("Index", "Home");
            }

            // Kullanıcı bilgilerini ve varsayılan modeli ayarla
            var model = new Rendevu
            {
                MusteriAd = user.UsrAd,
                MusteriSoyAd = user.UsrSoyad,
                MusteriMail = user.Email,
                MusteriTelefonNo = user.PhoneNumber
            };

            // Personel ve hizmet bilgilerini ViewBag ile doldurun
            ViewBag.PersonelListesi = new SelectList(
                _context.Personaller.Select(p => new { p.PersonelID, FullName = p.PersonelAd + " " + p.PersonelSoyAd }),
                "PersonelID", "FullName");

            ViewBag.HizmetListesi = _context.Hizmetler.Select(h => new SelectListItem
            {
                Value = h.HizmetID.ToString(),
                Text = h.HizmetAd
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult RandevuAl(Rendevu model, List<int> selectedHizmetler)
        {
            if (!ModelState.IsValid)
            {
                TempData["msj_kaytli"] = "Lütfen formu eksiksiz doldurun.";
                return RedirectToAction("RandevuAl");
            }

            // Yeni randevuyu oluştur
            var yeniRandevu = new Rendevu
            {
                MusteriAd = model.MusteriAd,
                MusteriSoyAd = model.MusteriSoyAd,
                MusteriMail = model.MusteriMail,
                MusteriTelefonNo = model.MusteriTelefonNo,
                RendevuZaman = model.RendevuZaman,
                PersonelID = model.PersonelID,
                RendevuOnayDurumu = false
            };

            _context.Rendevular.Add(yeniRandevu);
            _context.SaveChanges();

            // Hizmetleri kaydet
            if (selectedHizmetler != null && selectedHizmetler.Any())
            {
                foreach (var hizmetId in selectedHizmetler)
                {
                    _context.PersonelHizmetler.Add(new PersonelHizmet
                    {
                        PersonelID = model.PersonelID,
                        HizmetID = hizmetId
                    });
                }
                _context.SaveChanges();
            }

            TempData["msj_kaytli"] = "Randevu başarıyla kaydedildi.";
            return RedirectToAction("Index", "Home");
        }
    }
}
