using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tweetbook.Domain;

namespace Tweetbook.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Post>().HasMany<Tag>(t => t.Tags).WithOne(t => t.Post).HasForeignKey(t => t.PostId).OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(builder);
        }
    }
}
