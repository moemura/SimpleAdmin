using SimpleAdmin.Models.DTOs;
using SimpleAdmin.Models.Entities;

namespace SimpleAdmin.Models.Mapping
{
    public static class CategoryMapping
    {
        public static CategoryDto ToDto(this Category entity)
        {
            if (entity == null)
                return null;
            return new CategoryDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Image = entity.Image,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }

        public static Category ToEntity(this CategoryDto dto)
        {
            if (dto == null)
                return null;
            return new Category
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Image = dto.Image,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };
        }
    }
}