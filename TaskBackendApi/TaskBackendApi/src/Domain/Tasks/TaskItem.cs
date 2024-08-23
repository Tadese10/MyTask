using SharedKernel;

namespace Domain.Tasks;

public sealed class TaskItem : Entity
{
    public Guid UserId { get; set; }
    public string Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid ListId { get; set; }
    public Guid GroupId { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Priority Priority { get; set; }
    public string Title { get; set; }
    public DateTime? EndDate { get; set; }
    public List<Guid> AssignedUsers { get; set; }
    public Status Status { get; set; }

}
