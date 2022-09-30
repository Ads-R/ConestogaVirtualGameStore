using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ConestogaVirtualGameStore.Models
{
    public class GameStoreContext : IdentityDbContext<ApplicationUser>
    {
        public GameStoreContext(DbContextOptions<GameStoreContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //User table 1 to 1 relationship with Profile Table
            builder.Entity<ApplicationUser>()
                .HasOne<ProfileModel>(x => x.Profile)
                .WithOne(a => a.ApplicationUser)
                .HasForeignKey<ProfileModel>(b => b.UserId).IsRequired();
        }

        public DbSet<ProfileModel> Profiles { get; set; }
        public DbSet<GameModel> Games { get; set; }
    }
}
