using Auth.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) {}
    
    public DbSet<UserCredentials> UsersCredentials { get; set; }
    
    public DbSet<User> Users { get; set; }
}