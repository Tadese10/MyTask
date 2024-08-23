using Domain.List;

namespace Application.List.Get;

public sealed class ListResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Description { get; set; }
    public ListType ListType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int Count { get; internal set; }
}
