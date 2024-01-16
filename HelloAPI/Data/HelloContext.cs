using System.Reflection.Metadata;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
namespace HelloAPI.Data;

public class HelloContext: DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=my_host;Database=my_db;Username=my_user;Password=my_pw");
}
