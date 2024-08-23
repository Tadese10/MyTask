using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions.Data;
using Domain.List;
using Domain.Users;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class ListRepository : Database.MongoDb.MongoRepository<ListItem, Guid>, IListRepository
{
    //private readonly IMongoDbContext dbContext;
    public ListRepository(IMongoDbContext context, IMongoCollection<ListItem> dbSet) : base(context, dbSet)
    {
    }
}
