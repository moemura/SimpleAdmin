using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System.Text.Json;

namespace SimpleAdmin.Services.Files
{
    public class ImgurService : IImageStorageService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ImgurService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.DefaultRequestHeaders.Add("Authorization", _configuration["Imgur:ClientID"]);
        }

        public async Task<string> UploadImageAsync(IBrowserFile file)
        {
            try
            {
                using var content = new MultipartFormDataContent();
                using var fileStream = file.OpenReadStream(maxAllowedSize: 10485760); // 10MB
                using var streamContent = new StreamContent(fileStream);
                content.Add(streamContent, "image", file.Name);

                var response = await _httpClient.PostAsync(_configuration["Imgur:Url"], content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ImgurResponse>(responseContent);

                string link = result?.Data?.Link!;
                return link;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading image to Imgur: {ex.Message}");
            }
        }

        public async Task<List<string>> UploadImagesAsync(List<IBrowserFile> files)
        {
            var tasks = files.Select(file => UploadImageAsync(file));
            return (await Task.WhenAll(tasks)).ToList();
        }

        private class ImgurResponse
        {
            public ImgurData Data { get; set; }
        }

        private class ImgurData
        {
            public string Link { get; set; }
        }
    }
} 