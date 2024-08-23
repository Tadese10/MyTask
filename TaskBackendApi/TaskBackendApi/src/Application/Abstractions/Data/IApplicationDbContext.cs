using Domain.Tasks;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    IMongoCollection<User> Users { get; }
    IMongoCollection<TaskItem> TasksItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IApplicationDbContextMongoDb
{
    IMongoCollection<User> Users { get; }
    IMongoCollection<TaskItem> TasksItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

