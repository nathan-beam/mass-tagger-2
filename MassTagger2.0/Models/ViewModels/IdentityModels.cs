using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MassTagger2.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Hometown { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class MassTaggerContext : DbContext
    {
        public DbSet<RedditUser> RedditUsers { get; set; }

        public DbSet<Subreddit> Subreddits { get; set; }

        public DbSet<SubredditUser> SubredditUsers { get; set; }

        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubredditUser>()
                .HasRequired<Subreddit>(su => su.Subreddit)
                .WithMany(s => s.SubredditUsers)
                .HasForeignKey<int>(su => su.SubredditId);

            modelBuilder.Entity<SubredditUser>()
                .HasRequired<RedditUser>(su => su.User)
                .WithMany(r => r.SubredditUsers)
                .HasForeignKey<int>(su => su.UserId);

            modelBuilder.Entity<Post>()
                .HasRequired<SubredditUser>(p => p.SubredditUser)
                .WithMany(su => su.Posts)
                .HasForeignKey<int>(p => p.SubredditUserID);
        }
    }

}
