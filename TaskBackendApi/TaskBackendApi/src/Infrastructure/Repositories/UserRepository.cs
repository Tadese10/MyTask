using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions.Data;
using Domain.Users;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class UserRepository : Database.MongoDb.MongoRepository<User, Guid>, IUserRepository
{
    //private readonly IMongoDbContext dbContext;
    public UserRepository(IMongoDbContext context, IMongoCollection<User> dbSet) : base(context, dbSet)
    {
    }
}
