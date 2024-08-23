using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.List;
using Domain.Tasks;

namespace Application.Abstractions.Data;
public interface IListRepository : IRepository<ListItem, Guid>
{
}
