using SharedKernel;

namespace Domain.Tasks;

public sealed record TaskItemCompletedDomainEvent(Guid TaskItemId) : IDomainEvent;
