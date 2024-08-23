using Application.Abstractions.Messaging;

namespace Application.Group.Get;

public sealed record GetGroupQuery(): IQuery<List<GroupResponse>>;
