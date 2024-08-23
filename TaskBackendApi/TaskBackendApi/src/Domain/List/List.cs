using SharedKernel;

namespace Domain.List;

public sealed class ListItem : Entity
{
    public Guid CreatedByUserId { get; set; }
    public string Description { get; set; }
    public ListType ListType { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CompletedAt { get; set; }
}
