using SharedKernel;

namespace Domain.List;

public sealed record ListItemCompletedDomainEvent(Guid ListItemId) : IDomainEvent;
