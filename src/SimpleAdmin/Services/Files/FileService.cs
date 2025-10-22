using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;

namespace SimpleAdmin.Services.Files
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private const string UPLOAD_FOLDER = "images";

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveFileAsync(IBrowserFile file, string folder)
        {
            try
            {
                var uploadPath = Path.Combine(_environment.WebRootPath, UPLOAD_FOLDER, folder);
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.Name)}";
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = file.OpenReadStream(maxAllowedSize: 10485760)) // 10MB
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }

                return fileName;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving file: {ex.Message}");
            }
        }

        public async Task DeleteFileAsync(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    await Task.Run(() => File.Delete(filePath));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting file: {ex.Message}");
            }
        }

        public string GetFileUrl(string fileName, string folder)
        {
            if (string.IsNullOrEmpty(fileName))
                return string.Empty;

            return $"/{UPLOAD_FOLDER}/{folder}/{fileName}";
        }
    }
} 