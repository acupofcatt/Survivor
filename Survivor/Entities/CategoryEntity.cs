namespace Survivor.Entities;

public class CategoryEntity : BaseEntity
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public ICollection<CompetitorEntity>? Competitors { get; set; }
}