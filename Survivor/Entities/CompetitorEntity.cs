using System.ComponentModel.DataAnnotations.Schema;

namespace Survivor.Entities;

public class CompetitorEntity : BaseEntity
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public int CategoryId { get; set; }
    
    [ForeignKey(nameof(CategoryId))]
    public CategoryEntity Category { get; set; }
}