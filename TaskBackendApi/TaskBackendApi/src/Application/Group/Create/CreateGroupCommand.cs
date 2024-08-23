using Application.Abstractions.Messaging;
using Domain.Group;
using Domain.Tasks;

namespace Application.Group.Create;

public sealed class CreateGroupCommand : ICommand<Guid>
{
    public GroupType GroupType { get; set; }
    public string Name { get; set; }
}
