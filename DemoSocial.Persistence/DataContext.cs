using DemoSocial.Application.Abstractions;
using DemoSocial.Domain.Aggregates.PostAggregate;
using DemoSocial.Domain.Aggregates.UserProfileAggregate;
using DemoSocial.Persistence.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Persistence;

public class DataContext : IdentityDbContext, IDataContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PostCommentConfig());
        modelBuilder.ApplyConfiguration(new PostInteractionConfig());
        modelBuilder.ApplyConfiguration(new UserProfileConfig());
        modelBuilder.ApplyConfiguration(new IdentityUserConfig());
        modelBuilder.ApplyConfiguration(new IdentityUserRoleConfig());
        modelBuilder.ApplyConfiguration(new IdentityUserTokenConfig());
    }
}
