using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Web_Programlama__Proje.Controllers
{
    public class HuggingController : Controller
    {
        private readonly HttpClient _httpClient;

        public HuggingController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public IActionResult TextToImage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TextToImage(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                ModelState.AddModelError("prompt", "Lütfen bir açıklama girin.");
                return View();
            }

            string apiUrl = "https://api-inference.huggingface.co/models/ZB-Tech/Text-to-Image";
            string apiKey = ""; // Replace with your API key

            try
            {
                // API'ye gönderilecek JSON içeriği oluşturma
                var payload = new { inputs = prompt };
                var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                var response = await _httpClient.PostAsync(apiUrl, jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"API hatası: {errorMessage}");
                    return View();
                }

                // API yanıtını al ve blob olarak işle
                var imageBytes = await response.Content.ReadAsByteArrayAsync();
                string base64Image = Convert.ToBase64String(imageBytes);

                // Görseli ViewBag ile döndür
                ViewBag.ImageUrl = $"data:image/png;base64,{base64Image}";
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Bir hata oluştu: {ex.Message}");
                return View();
            }
        }
    }
}
