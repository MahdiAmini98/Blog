using Blog.PanelAdmin.Models.Medias;
using Blog.PanelAdmin.Models.Pagination;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Blog.PanelAdmin.Services.Medias
{
    public interface IMediaService
    {
        Task<PaginatedList<MediaDto>> GetAllMediaAsync(int page, int pageSize);
        Task<MediaDto?> GetMediaByIdAsync(Guid id);
        Task<bool> UploadMediaAsync(Stream fileStream, string fileName, string type);
        Task<bool> DeleteMediaAsync(Guid id);
    }
    public class MediaService : IMediaService
    {
        private readonly HttpClient _httpClient;

        public MediaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedList<MediaDto>> GetAllMediaAsync(int page, int pageSize)
        {
            return await _httpClient.GetFromJsonAsync<PaginatedList<MediaDto>>($"api/media?page={page}&pageSize={pageSize}")
                   ?? new PaginatedList<MediaDto>(new List<MediaDto>(), 0, page, pageSize);
        }

        public async Task<MediaDto?> GetMediaByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<MediaDto>($"api/media/{id}");
        }

        public async Task<bool> UploadMediaAsync(Stream fileStream, string fileName, string type)
        {
            using var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

            content.Add(fileContent, "Files", fileName);
            content.Add(new StringContent(type), "Type");

            var response = await _httpClient.PostAsync("api/media/upload", content);
            return response.IsSuccessStatusCode;
        }


        public async Task<bool> DeleteMediaAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/media/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
