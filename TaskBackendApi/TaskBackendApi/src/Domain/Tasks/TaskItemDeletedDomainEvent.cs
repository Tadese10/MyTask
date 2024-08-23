using SharedKernel;

namespace Domain.Tasks;

public sealed record TaskItemDeletedDomainEvent(Guid TasksItemId) : IDomainEvent;
