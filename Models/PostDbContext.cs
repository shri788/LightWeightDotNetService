using Microsoft.EntityFrameworkCore;

namespace LightWeightDotNetService.Models
{
    public class PostDbContext : DbContext
    {
        public PostDbContext(DbContextOptions<PostDbContext> options)
            : base(options) { }

        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Api> Apis { get; set; } = null!;

    }
}