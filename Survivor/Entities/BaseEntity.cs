namespace Survivor.Entities;

public class BaseEntity
{
    public DateTimeOffset CreatedDate { get; set; }

    public DateTimeOffset ModifiedDate { get; set; }
    
    public bool IsDeleted { get; set; }
}