using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Web_Programlama__Proje.Models;

namespace Web_Programlama__Proje.Controllers
{
    public class RendevuConsumeApiController : Controller
    {
        //RendevuContext _context = new RendevuContext();

        public async Task<IActionResult> RendevuGetir()
        {
            List<Rendevu> rendevular = new List<Rendevu>();
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://localhost:7187/Api/RestApi");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                rendevular = JsonConvert.DeserializeObject<List<Rendevu>>(jsonData);
                return View(rendevular);
            }
            return NotFound("Veriler alınamadı.");
        }

        public IActionResult Index2()
        {
            return View();
        }

        // POST işlemi: Yeni randevu ekleme
        [HttpPost]
        public async Task<IActionResult> RendevuEkle(Rendevu yeniRandevu)
        {
            var httpClient = new HttpClient();
            var jsonData = JsonConvert.SerializeObject(yeniRandevu);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://localhost:7187/Api/RestApi", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("RendevuGetir");
            }
            return BadRequest("Randevu ekleme başarısız.");
        }

        // PUT işlemi: Randevu güncelleme
        [HttpPut]
        public async Task<IActionResult> RendevuGuncelle(int id, Rendevu guncellenenRandevu)
        {
            var httpClient = new HttpClient();
            var jsonData = JsonConvert.SerializeObject(guncellenenRandevu);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"https://localhost:7187/Api/RestApi/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("RendevuGetir");
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound("Güncellenecek randevu bulunamadı.");
            }
            return BadRequest("Randevu güncelleme başarısız.");
        }

        // DELETE işlemi: Randevu silme
        [HttpDelete]
        public async Task<IActionResult> RendevuSil(int id)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.DeleteAsync($"https://localhost:7187/Api/RestApi/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("RendevuGetir");
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound("Silinecek randevu bulunamadı.");
            }
            return BadRequest("Randevu silme başarısız.");
        }
    }
}
