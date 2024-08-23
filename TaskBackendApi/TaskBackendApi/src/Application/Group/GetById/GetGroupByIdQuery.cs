using Application.Abstractions.Messaging;
using Application.Group.Get;
using Application.List.Get;

namespace Application.Group.GetById;

public sealed record GetGroupByIdQuery(Guid GroupItemId) : IQuery<GroupResponse>;
