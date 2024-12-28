
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Web_Programlama__Proje.Controllers
{
    [Authorize(Roles = "Admin,Kullanici")]
    public class RapController : Controller
    {
        private readonly HttpClient _httpClient;

        public RapController(IHttpClientFactory httpClientFactory)
        {
            // IHttpClientFactory ile HttpClient oluştur
            _httpClient = httpClientFactory.CreateClient();
        }

        // GET: Sadece formu döndürür
        [HttpGet]
        public IActionResult ImageToImage()
        {
            return View();
        }

        // POST: Fotoğrafı ve 5 farklı hair_type isteğini işler
        [HttpPost]
        public async Task<IActionResult> ImageToImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("imageFile", "Lütfen geçerli bir resim dosyası yükleyin.");
                return View();
            }

            // 1) Fotoğrafı byte[] olarak oku
            byte[] imageBytes;
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            // Kullanıcıdan yüklenen fotoğrafı (base64) olarak saklayıp sayfada gösterelim
            ViewBag.UploadedImageBase64 = Convert.ToBase64String(imageBytes);

            // 2) Saç tipleri listesi
            //  (Siz 5 veya daha fazla ekleyebilirsiniz)
            var hairTypes = new List<string> { "201", "1001", "201", "801", "1301" };

            // API’den dönen 5 farklı sonucun base64 görselini saklayacağız
            var hairStyleResults = new Dictionary<string, string>();

            // RapidAPI bilgileri
            string apiUrl = "https://hairstyle-changer.p.rapidapi.com/huoshan/facebody/hairstyle";
            string rapidApiKey = "1713462df4msh2e908d90d77b96cp100b74jsn13a284d29675";
            string rapidApiHost = "hairstyle-changer.p.rapidapi.com";

            try
            {
                // Her bir saç tipi için tek tek istek at
                foreach (var hairType in hairTypes)
                {
                    // multipart/form-data oluştur
                    var formData = new MultipartFormDataContent();

                    formData.Add(new StringContent(hairType), "hair_type");

                    var fileContent = new ByteArrayContent(imageBytes);
                    // Örnek: "image/png" ya da "image/jpeg"
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
                    formData.Add(fileContent, "image_target", imageFile.FileName);

                    // RapidAPI header’larını ekle
                    _httpClient.DefaultRequestHeaders.Clear();
                    _httpClient.DefaultRequestHeaders.Add("x-rapidapi-key", rapidApiKey);
                    _httpClient.DefaultRequestHeaders.Add("x-rapidapi-host", rapidApiHost);

                    // POST isteği gönder
                    var response = await _httpClient.PostAsync(apiUrl, formData);
                    if (!response.IsSuccessStatusCode)
                    {
                        // API hatası durumunda ilgili saç tipi için "null" döndürür
                        hairStyleResults[hairType] = null;
                        continue;
                    }

                    // JSON içeriği oku
                    var responseContent = await response.Content.ReadAsStringAsync();

                    using var jsonDoc = JsonDocument.Parse(responseContent);
                    if (jsonDoc.RootElement.TryGetProperty("data", out JsonElement dataElem) &&
                        dataElem.TryGetProperty("image", out JsonElement imageElem))
                    {
                        var base64Image = imageElem.GetString();
                        // "data:image/png;base64,..." formatına çevirerek kaydediyoruz
                        hairStyleResults[hairType] = $"data:image/png;base64,{base64Image}";
                    }
                    else
                    {
                        // "image" alanı bulunamadıysa null atayalım
                        hairStyleResults[hairType] = null;
                    }
                }

                // Tüm sonuçları ViewBag’e koy
                // Key = hair_type, Value = base64 veri (veya null)
                ViewBag.HairStyleResults = hairStyleResults;

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
