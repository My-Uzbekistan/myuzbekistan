using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("/uploads")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public UploadController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var minioUrl = $"https://minio.uzdc.uz/myzubekistan/uploads/{id}";

            // Выполняем запрос к MinIO
            var response = await _httpClient.GetAsync(minioUrl);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to fetch file from MinIO.");
            }

            // Возвращаем файл клиенту
            var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";
            var fileStream = await response.Content.ReadAsStreamAsync();
            return File(fileStream, contentType);
        }
    }
}
