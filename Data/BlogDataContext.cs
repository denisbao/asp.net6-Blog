using Microsoft.EntityFrameworkCore;
using NewBlog.Models;

namespace Blog.Data
{
  public class BlogDataContext : DbContext
  {
    // Definição da representação das tabelas:
    public DbSet<Category> Categories { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Tag> Tags { get; set; }
  }
}