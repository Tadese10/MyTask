using SharedKernel;

namespace Domain.Group;

public sealed record GroupItemCreatedDomainEvent(Guid GroupItemId) : IDomainEvent;
