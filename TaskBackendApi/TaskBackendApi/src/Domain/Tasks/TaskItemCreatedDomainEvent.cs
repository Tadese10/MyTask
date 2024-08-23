using SharedKernel;

namespace Domain.Tasks;

public sealed record TaskItemCreatedDomainEvent(Guid TasksItemId) : IDomainEvent;
