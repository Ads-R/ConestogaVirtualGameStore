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
            //User table 1 to 1 relationship with Preference Table
            builder.Entity<ApplicationUser>()
                .HasOne<PreferencesModel>(x => x.Preference)
                .WithOne(a => a.ApplicationUser)
                .HasForeignKey<PreferencesModel>(b => b.UserId).IsRequired();
            //User table 1 to 1 relationship with Address Table
            builder.Entity<ApplicationUser>()
                .HasOne<AddressModel>(x => x.Address)
                .WithOne(a => a.ApplicationUser)
                .HasForeignKey<AddressModel>(b => b.UserId).IsRequired();

            builder.Entity<ProfileModel>()
                .Property(x => x.Gender)
                .HasConversion<string>()
                .HasMaxLength(20);
        }

        public DbSet<ProfileModel> Profiles { get; set; }
        public DbSet<GameModel> Games { get; set; }
        public DbSet<PreferencesModel> Preferences { get; set; }
        public DbSet<AddressModel> Address { get; set; }
    }
}
