using Application.Abstractions.Messaging;
using Application.List.Get;

namespace Application.List.GetById;

public sealed record GetListByIdQuery(Guid ListItemId) : IQuery<ListResponse>;
