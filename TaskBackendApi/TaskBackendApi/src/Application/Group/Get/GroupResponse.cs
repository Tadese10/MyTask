using Domain.Group;
using Domain.List;

namespace Application.Group.Get;

public sealed class GroupResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public GroupType GroupType { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }

}
