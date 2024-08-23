using Domain.Group;
using Domain.List;

namespace Application.Group.GetById;

public sealed class ListResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public GroupType GroupType { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
}
