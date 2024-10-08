﻿using Domain.List;

namespace Application.List.GetById;

public sealed class ListResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Description { get; set; }
    public ListType ListType { get; set; }
    public List<string> Labels { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
