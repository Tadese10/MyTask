using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Users;

namespace Application.Abstractions.Data;
public interface IUserRepository : IRepository<User, Guid>
{

}
