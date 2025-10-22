using Microsoft.EntityFrameworkCore;
using SimpleAdmin.Services.Catches;

namespace SimpleAdmin.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        private readonly ICacheService _cacheService;
        private const string CACHE_PREFIX = "Category_";

        public CategoryService(IDbContextFactory<ApplicationDbContext> dbContextFactory, ICacheService cacheService)
        {
            _dbContextFactory = dbContextFactory;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<CategoryDto>> GetAll()
        {
            var cacheKey = $"{CACHE_PREFIX}All";
            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                using var dbContext = await _dbContextFactory.CreateDbContextAsync();
                var data = await dbContext.Categories.ToListAsync();
                return data.Select(c => c.ToDto());
            });
        }

        public async Task<CategoryDto> GetById(string Id)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var category = await dbContext.Categories.SingleOrDefaultAsync(c => c.Id == Id);
            return category.ToDto();
        }

        public async Task<CategoryDto> Create(CategoryDto dto)
        {
            if (dto == null)
                throw new Exception("Data must not null!");
            
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var category = dto.ToEntity();
            category.Id = Guid.CreateVersion7().ToString();
            category.CreatedAt = DateTime.UtcNow;
            category.UpdatedAt = DateTime.UtcNow;
            await dbContext.Categories.AddAsync(category);
            dto = category.ToDto();
            await dbContext.SaveChangesAsync();
            await _cacheService.RemoveByPrefixAsync(CACHE_PREFIX);
            return dto;
        }

        public async Task Update(CategoryDto dto)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == dto.Id)
                ?? throw new Exception("Category not found!");
            category.UpdatedAt = DateTime.UtcNow;
            category.Name = dto.Name;
            category.Description = dto.Description;
            category.Image = dto.Image;

            dbContext.Update(category);
            await dbContext.SaveChangesAsync();
            await _cacheService.RemoveByPrefixAsync(CACHE_PREFIX);
        }

        public async Task Delete(string id)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new Exception("Category not found!");
            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync();
            await _cacheService.RemoveByPrefixAsync(CACHE_PREFIX);
        }

        public async Task<IEnumerable<CategoryDto>> Filter(Dictionary<string, string> filter)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var query = dbContext.Categories.AsQueryable();

            if (filter.ContainsKey("name") && !string.IsNullOrEmpty(filter["name"]))
            {
                query = query.Where(c => c.Name.Contains(filter["name"]));
            }

            var categories = await query.ToListAsync();
            return categories.Select(c => c.ToDto());
        }

        public async Task<PaginatedList<CategoryDto>> FilterAndPagin(int pageIndex, int pageSize, Dictionary<string, string> filter)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var query = dbContext.Categories.AsQueryable();

            if (filter.ContainsKey("name") && !string.IsNullOrEmpty(filter["name"]))
            {
                query = query.Where(c => c.Name.Contains(filter["name"]));
            }

            var totalItems = await query.CountAsync();
            var categories = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<CategoryDto>(
                categories.Select(c => c.ToDto()),
                pageIndex,
                pageSize,
                totalItems
            );
        }

        public async Task<PaginatedList<CategoryDto>> GetPagination(int pageIndex, int pageSize)
        {
            var cacheKey = $"{CACHE_PREFIX}Page_{pageIndex}_Size_{pageSize}";
            return await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                using var dbContext = await _dbContextFactory.CreateDbContextAsync();
                var totalItems = await dbContext.Categories.CountAsync();
                var categories = await dbContext.Categories
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PaginatedList<CategoryDto>(
                    categories.Select(c => c.ToDto()),
                    pageIndex,
                    pageSize,
                    totalItems
                );
            });
        }
    }
} 