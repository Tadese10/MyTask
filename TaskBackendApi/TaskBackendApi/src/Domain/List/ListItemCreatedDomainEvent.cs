using SharedKernel;

namespace Domain.List;

public sealed record ListItemCreatedDomainEvent(Guid ListItemId) : IDomainEvent;
