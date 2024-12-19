using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace Web_Programlama__Proje.Controllers
{
    public class ImageController : Controller
    {
        private readonly HttpClient _httpClient;

        public ImageController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public IActionResult TransformImage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TransformImage(IFormFile inputImage, string prompt)
        {
            if (inputImage == null || inputImage.Length == 0)
                return BadRequest("Lütfen bir başlangıç görseli yükleyin.");

            if (string.IsNullOrWhiteSpace(prompt))
                return BadRequest("Lütfen bir açıklama girin.");

            // Hugging Face API anahtarınızı ekleyin
            string apiKey = "";
            // Image-to-Image destekleyen bir model kullanın. Örneğin stable diffusion image-to-image:
            string apiUrl = "https://api-inference.huggingface.co/models/kandinsky-community/kandinsky-2-2-decoder";

            try
            {
                using var memoryStream = new MemoryStream();
                await inputImage.CopyToAsync(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();

                if (imageBytes.Length == 0)
                    return BadRequest("Gönderilen görsel boş.");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                // Bu modelin çıktısının görüntü olarak döndüğünü varsayıyoruz:
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/png"));

                // Görseli data URI formatına çevir
                string base64Image = Convert.ToBase64String(imageBytes);
                string dataUri = $"data:image/png;base64,{base64Image}";

                // Modelin beklediği format için huggingface model sayfasına bakın.
                // Stable diffusion image-to-image genelde şu formatı kabul eder:
                // { "init_image": "data:image/png;base64,...", "prompt": "your prompt" }
                var requestData = new
                {
                    init_image = dataUri,
                    prompt = prompt
                };

                var json = JsonSerializer.Serialize(requestData);
                var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(apiUrl, jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"API isteği başarısız oldu: {errorMessage}");
                }

                // Bu model çıktıyı doğrudan binary resim olarak döndürebilir.
                var imageResponseBytes = await response.Content.ReadAsByteArrayAsync();
                string base64OutputImage = Convert.ToBase64String(imageResponseBytes);
                ViewBag.ImageUrl = $"data:image/png;base64,{base64OutputImage}";

                return View();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Bir hata oluştu: " + ex.Message);
            }
        }
    }
}
