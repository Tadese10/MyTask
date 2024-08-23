using Domain.Group;
using Domain.Tasks;

namespace Application.Taskss.GetById;

public sealed class TasksResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Priority Priority { get; set; }
    public Guid ListId { get; set; }
    public Guid GroupId { get; set; }
    public List<Guid> AssignedUsers { get; set; }
}
