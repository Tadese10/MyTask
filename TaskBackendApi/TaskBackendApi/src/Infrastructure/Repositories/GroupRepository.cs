using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions.Data;
using Domain.Group;
using Domain.List;
using Domain.Users;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class GroupRepository : Database.MongoDb.MongoRepository<GroupItem, Guid>, IGroupRepository
{
    //private readonly IMongoDbContext dbContext;
    public GroupRepository(IMongoDbContext context, IMongoCollection<GroupItem> dbSet) : base(context, dbSet)
    {
    }
}
