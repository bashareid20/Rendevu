using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

public class OpenAiApiController: Controller
{
    private readonly HttpClient _httpClient;

    public OpenAiApiController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpGet]
    public IActionResult ImageToImage()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ImageToImage(string prompt, IFormFile imageFile, IFormFile maskFile = null)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            ModelState.AddModelError("prompt", "Lütfen bir açıklama girin.");
            return View();
        }

        if (imageFile == null || imageFile.Length == 0)
        {
            ModelState.AddModelError("imageFile", "Lütfen geçerli bir görüntü dosyası yükleyin.");
            return View();
        }

        string apiUrl = "https://api.openai.com/v1/images/edits";
        string apiKey = "";

        try
        {
            // Görüntü dosyasını byte dizisine dönüştür
            byte[] imageBytes;
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            // Mask dosyasını dönüştür (varsa)
            byte[] maskBytes = null;
            if (maskFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await maskFile.CopyToAsync(memoryStream);
                    maskBytes = memoryStream.ToArray();
                }
            }

            // Multipart form data hazırlama
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(prompt), "prompt");
            formData.Add(new ByteArrayContent(imageBytes) { Headers = { ContentType = new MediaTypeHeaderValue("image/png") } }, "image", imageFile.FileName);

            if (maskBytes != null)
            {
                formData.Add(new ByteArrayContent(maskBytes) { Headers = { ContentType = new MediaTypeHeaderValue("image/png") } }, "mask", maskFile.FileName);
            }

            // Authorization header ekle
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            // API'ye isteği gönder
            var response = await _httpClient.PostAsync(apiUrl, formData);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"API Hatası: {errorMessage}");
                return View();
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonDocument.Parse(responseContent);
            var imageUrl = jsonResponse.RootElement.GetProperty("data")[0].GetProperty("url").GetString();

            ViewBag.ImageUrl = imageUrl;
            return View();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Bir hata oluştu: {ex.Message}");
            return View();
        }
    }
}
