using SimpleAdmin.Models;

namespace SimpleAdmin.Services.Categories
{
    public interface ICategoryService
    {
        Task<CategoryDto> Create(CategoryDto dto);
        Task Delete(string id);
        Task<IEnumerable<CategoryDto>> Filter(Dictionary<string, string> filter);
        Task<PaginatedList<CategoryDto>> FilterAndPagin(int pageIndex, int pageSize, Dictionary<string, string> filter);
        Task<IEnumerable<CategoryDto>> GetAll();
        Task<CategoryDto> GetById(string Id);
        Task<PaginatedList<CategoryDto>> GetPagination(int pageIndex, int pageSize);
        Task Update(CategoryDto dto);
    }
} 