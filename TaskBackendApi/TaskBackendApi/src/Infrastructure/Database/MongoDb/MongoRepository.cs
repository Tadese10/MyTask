using System.Linq.Expressions;
using Domain;
using MongoDB.Driver;
using SharedKernel;

namespace Infrastructure.Database.MongoDb;

public class MongoRepository<TEntity, TId> : Application.Abstractions.Data.IMongoRepository<TEntity, TId>
    where TEntity : class, IBaseEntity<TId>
{
    private readonly Application.Abstractions.Data.IMongoDbContext _context;
    protected readonly IMongoCollection<TEntity> DbSet;

    public MongoRepository(Application.Abstractions.Data.IMongoDbContext context, IMongoCollection<TEntity> dbSet)
    {
        _context = context;
        DbSet = dbSet;
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.InsertOneAsync(entity, new InsertOneOptions(), cancellationToken);
        return entity;
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    public Task<TEntity?> FindByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return FindOneAsync(e => e.Id!.Equals(id), cancellationToken);
    }

    public Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return DbSet.Find(predicate).SingleOrDefaultAsync(cancellationToken: cancellationToken)!;
    }

    public async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
       return await DbSet.Find(predicate).ToListAsync(cancellationToken: cancellationToken)!;
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.AsQueryable().ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await DbSet.Find(predicate).AnyAsync(cancellationToken: cancellationToken)!;
    }

    async Task Application.Abstractions.Data.IWriteRepository<TEntity, TId>.AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await DbSet.InsertOneAsync(entity, new InsertOneOptions(), cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _ = await DbSet.ReplaceOneAsync(x => x.Id!.Equals(entity.Id), entity, cancellationToken: cancellationToken);
    }

    public Task DeleteRangeAsync(IReadOnlyList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        await DbSet.DeleteOneAsync(d => d.Id!.Equals(id), cancellationToken);
    }
}
