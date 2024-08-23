using SharedKernel;

namespace Domain.List;

public sealed record ListItemDeletedDomainEvent(Guid ListItemId) : IDomainEvent;
