using Application.Abstractions.Data;
using Domain.Group;
using Domain.List;
using Domain.Tasks;
using Domain.Users;
using Infrastructure.Caching;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Infrastructure.Database.MongoDb;

public static class Extensions
{
    public static IServiceCollection AddMongoDbContext<TContext>(
        this IServiceCollection services, IConfiguration configuration)
        where TContext : MongoDbContext
    {
        return services.AddMongoDbContext<TContext, TContext>(configuration);
    }

    public static IServiceCollection AddMongoDbContext<TContextService, TContextImplementation>(
        this IServiceCollection services, IConfiguration configuration)
        where TContextService : Application.Abstractions.Data.IMongoDbContext
        where TContextImplementation : MongoDbContext, TContextService
    
    {
        services.Configure<MongoOptions>(configuration.GetSection(nameof(MongoOptions)));

        services.AddScoped(typeof(TContextService), typeof(TContextImplementation));
        services.AddScoped(typeof(TContextImplementation));

        services.AddScoped<IMongoDbContext>(sp => sp.GetRequiredService<TContextService>());

        services.AddTransient(typeof(IMongoRepository<,>), typeof(MongoRepository<,>));

        services.AddTransient<IUserRepository>(sp => {

            IMongoDbContext dbContext =  sp.GetRequiredService<IMongoDbContext>();

            return new UserRepository(dbContext, dbContext.GetCollection<User>());
            });

        services.AddTransient<ITasksRepository>(sp => {

            IMongoDbContext dbContext = sp.GetRequiredService<IMongoDbContext>();

            return new TasksRepository(dbContext, dbContext.GetCollection<TaskItem>());
        });

        services.AddTransient<IListRepository>(sp => {

            IMongoDbContext dbContext = sp.GetRequiredService<IMongoDbContext>();

            return new ListRepository(dbContext, dbContext.GetCollection<ListItem>());
        });

        services.AddTransient<IGroupRepository>(sp => {

            IMongoDbContext dbContext = sp.GetRequiredService<IMongoDbContext>();

            return new GroupRepository(dbContext, dbContext.GetCollection<GroupItem>());
        });

        services.AddTransient<ICacheService>(sp => {

            IConnectionMultiplexer conn = sp.GetRequiredService<IConnectionMultiplexer>();

            ILogger<CacheService> logg = sp.GetRequiredService<ILogger<CacheService>>();


            return new CacheService(conn, logg);
        });

        return services;
    }
}
