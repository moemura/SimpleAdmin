using Microsoft.AspNetCore.Components.Forms;

namespace SimpleAdmin.Services.Files
{
    public interface IImageStorageService
    {
        Task<string> UploadImageAsync(IBrowserFile file);
        Task<List<string>> UploadImagesAsync(List<IBrowserFile> files);
    }
} 