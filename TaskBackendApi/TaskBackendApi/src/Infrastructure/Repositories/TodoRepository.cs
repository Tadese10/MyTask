using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Tasks;
using Domain.Users;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class TasksRepository : Database.MongoDb.MongoRepository<TaskItem, Guid>, Application.Abstractions.Data.ITasksRepository
{
    public TasksRepository(Application.Abstractions.Data.IMongoDbContext context, IMongoCollection<TaskItem> dbSet) : base(context, dbSet)
    {
    }
}
