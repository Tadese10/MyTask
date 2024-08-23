using SharedKernel;

namespace Domain.Group;

public sealed record GroupItemCompletedDomainEvent(Guid GroupItemId) : IDomainEvent;
