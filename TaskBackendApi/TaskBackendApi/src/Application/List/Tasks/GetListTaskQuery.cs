using Application.Abstractions.Messaging;

namespace Application.List.Tasks;

public sealed record GetListTaskQuery(Guid ListItemId): IQuery<object>;
