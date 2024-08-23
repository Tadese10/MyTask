using SharedKernel;

namespace Domain.Group;

public sealed class GroupItem : Entity
{
    public Guid CreatedByUserId { get; set; }
    public GroupType GroupType { get; set; }
    public string Name { get; set; } 
}
