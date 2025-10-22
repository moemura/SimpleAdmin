using System.ComponentModel.DataAnnotations;

namespace SimpleAdmin.Models.Entities;

public abstract class BaseEntity<TKey>
{
    [Key]
    public TKey Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    [MaxLength(36)]
    public string? CreatedBy { get; set; }
    [MaxLength(36)]
    public string? UpdatedBy { get; set; }
}
