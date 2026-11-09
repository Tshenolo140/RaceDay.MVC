using RaceDay.MVC.Models;

namespace RaceDay.MVC.Services
{
    public interface IApiService
    {
        Task<AuthResponseViewModel?> LoginAsync(LoginViewModel login);
        Task<bool> RegisterAsync(RegisterViewModel register);
        Task<List<EventViewModel>> GetEventsAsync();
        Task<EventViewModel?> GetEventAsync(int id);
        Task CreateEventAsync(EventViewModel ev);
        Task<List<ResultViewModel>> GetMyResultsAsync();
        Task<string> UploadEventBannerAsync(IFormFile file);
    }
}