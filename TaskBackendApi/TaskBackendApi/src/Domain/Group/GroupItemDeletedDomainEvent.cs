using SharedKernel;

namespace Domain.Group;

public sealed record GroupItemDeletedDomainEvent(Guid ListItemId) : IDomainEvent;
