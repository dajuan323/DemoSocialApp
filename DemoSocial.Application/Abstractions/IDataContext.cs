using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.Abstractions;

public interface IDataContext
{
    DbSet<UserProfile> UserProfiles { get; set; }
    DbSet<Post> Posts { get; set; }
}
