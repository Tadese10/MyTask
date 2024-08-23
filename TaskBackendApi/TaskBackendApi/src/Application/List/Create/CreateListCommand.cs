using Application.Abstractions.Messaging;
using Domain.Tasks;

namespace Application.List.Create;

public sealed class CreateListCommand : ICommand<Guid>
{
    public Guid UserId { get; set; }
    public string Description { get; set; }
    public Domain.List.ListType ListType { get; set; }
}
