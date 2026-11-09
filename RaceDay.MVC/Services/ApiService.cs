using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using RaceDay.MVC.Models;

namespace RaceDay.MVC.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _httpContext;

        public ApiService(HttpClient http, IHttpContextAccessor httpContext)
        {
            _http = http;
            _httpContext = httpContext;
        }

        private void AttachToken()
        {
            var token = _httpContext.HttpContext?.Session.GetString("JWTToken");
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<AuthResponseViewModel?> LoginAsync(LoginViewModel login)
        {
            var response = await _http.PostAsJsonAsync("auth/login", login);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<AuthResponseViewModel>();
        }

        public async Task<bool> RegisterAsync(RegisterViewModel register)
        {
            var response = await _http.PostAsJsonAsync("auth/register", register);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<EventViewModel>> GetEventsAsync()
        {
            AttachToken();
            return await _http.GetFromJsonAsync<List<EventViewModel>>("events") ?? new();
        }

        public async Task<EventViewModel?> GetEventAsync(int id)
        {
            AttachToken();
            return await _http.GetFromJsonAsync<EventViewModel>($"events/{id}");
        }

        public async Task CreateEventAsync(EventViewModel ev)
        {
            AttachToken();
            await _http.PostAsJsonAsync("events", ev);
        }

        public async Task<List<ResultViewModel>> GetMyResultsAsync()
        {
            AttachToken();
            return await _http.GetFromJsonAsync<List<ResultViewModel>>("results/my-results") ?? new();
        }

        public async Task<string> UploadEventBannerAsync(IFormFile file)
        {
            AttachToken();
            using var content = new MultipartFormDataContent();
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            var fileContent = new ByteArrayContent(stream.ToArray());
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
            content.Add(fileContent, "file", file.FileName);

            var response = await _http.PostAsync("events/upload-banner", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                return result?["url"] ?? "";
            }
            return "";
        }
    }
}