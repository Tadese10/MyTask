using Domain.Tasks;

namespace Application.Taskss.Get;

public sealed class TaskResponse
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
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Status Status { get; set; }
}
