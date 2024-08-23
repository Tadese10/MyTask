using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Caching;
public class RedisOptions
{
    public string Url { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Username { get; set; } = null!;
}
