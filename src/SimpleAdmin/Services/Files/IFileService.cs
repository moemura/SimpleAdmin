using Microsoft.AspNetCore.Components.Forms;

namespace SimpleAdmin.Services.Files
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IBrowserFile file, string folder);
        Task DeleteFileAsync(string filePath);
        string GetFileUrl(string fileName, string folder);
    }
} 