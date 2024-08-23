﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Tasks;
using Domain.Users;

namespace Application.Abstractions.Data;
public interface ITasksRepository : IRepository<TaskItem, Guid>
{
}
