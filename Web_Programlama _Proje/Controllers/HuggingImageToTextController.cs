using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class HuggingImageToTextController : Controller
{
    private readonly HttpClient _httpClient;
    private const string OpenAiApiUrl = "https://api.openai.com/v1/chat/completions"; // OpenAI ChatGPT API URL
    private const string OpenAiApiKey = ""; // OpenAI API Anahtarınızı buraya ekleyin

    public HuggingImageToTextController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpGet]
    public IActionResult ImageToText()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ImageToText(string prompt, IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            ModelState.AddModelError("imageFile", "Lütfen geçerli bir fotoğraf yükleyin.");
            return View();
        }

        if (string.IsNullOrWhiteSpace(prompt))
        {
            ModelState.AddModelError("prompt", "Lütfen bir açıklama yazın.");
            return View();
        }

        try
        {
            // Görseli Base64 formatına dönüştür
            byte[] imageBytes;
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }
            string base64Image = Convert.ToBase64String(imageBytes);

            // OpenAI için prompt oluştur
            string fullPrompt = $"Kullanıcı aşağıdaki açıklamayı sağladı: \"{prompt}\". Ayrıca bir görsel yükledi. Görselin Base64 formatı aşağıdadır:\n\n{base64Image}\n\n" +
                                "Bu görsel ve açıklamaya dayanarak,ne göryosan yaz listeln.";

            // OpenAI API için JSON payload oluştur
            var payload = new
            {
                model = "gpt-3.5-turbo-16k", // Model olarak GPT-3.5-Turbo-16k kullanılıyor
                messages = new[]
                {
                    new { role = "system", content = "You are a hair stylist assistant who provides hair styling and color recommendations based on user inputs and images." },
                    new { role = "user", content = fullPrompt }
                },
                max_tokens = 300, // Yanıt uzunluğu
                temperature = 0.7
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            // Authorization header ekle
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", OpenAiApiKey);

            // OpenAI API'ye isteği gönder
            var response = await _httpClient.PostAsync(OpenAiApiUrl, jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"OpenAI API Hatası: {errorMessage}");
                return View();
            }

            // API'den dönen yanıtı JSON olarak işle
            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonDocument.Parse(responseContent);
            var generatedText = jsonResponse.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

            // Yanıtı ViewBag ile döndür
            ViewBag.GeneratedText = generatedText;
            return View();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Bir hata oluştu: {ex.Message}");
            return View();
        }
    }
}
