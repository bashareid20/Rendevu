using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Web_Programlama__Proje.Controllers
{
    public class ImageTImageController : Controller
    {
        private readonly HttpClient _httpClient;

        public ImageTImageController(IHttpClientFactory httpClientFactory)
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
            {
                return BadRequest("Lütfen bir görsel yükleyin.");
            }

            if (string.IsNullOrWhiteSpace(prompt))
            {
                return BadRequest("Lütfen bir açıklama girin.");
            }

            string apiKey = ""; // Hugging Face API anahtarınızı buraya ekleyin
            string apiUrl = "https://api-inference.huggingface.co/models/kandinsky-community/kandinsky-2-2-decoder";

            try
            {
                // Görseli byte array'e dönüştür
                using var memoryStream = new MemoryStream();
                await inputImage.CopyToAsync(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                // Multipart form-data içeriği oluştur
                var formData = new MultipartFormDataContent
                {
                    {
                        new ByteArrayContent(imageBytes)
                        {
                            Headers =
                            {
                                ContentType = new MediaTypeHeaderValue("image/png")
                            }
                        },
                        "init_image",
                        inputImage.FileName
                    },
                    { new StringContent(prompt), "prompt" }
                };

                // API isteği gönder
                var response = await _httpClient.PostAsync(apiUrl, formData);

                if (!response.IsSuccessStatusCode)
                {
                    // Hata mesajını okuyup döndür
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"API hatası: {errorMessage}");
                }

                // Yanıtı binary olarak al ve Base64 string'e dönüştür
                var responseBytes = await response.Content.ReadAsByteArrayAsync();
                string base64Image = Convert.ToBase64String(responseBytes);

                // Görseli ViewBag ile döndür
                ViewBag.ImageUrl = $"data:image/png;base64,{base64Image}";
                return View();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }
    }
}
