﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ConestogaVirtualGameStore.Models
{
    public class GameStoreContext : IdentityDbContext<ApplicationUser>
    {
        public GameStoreContext(DbContextOptions<GameStoreContext> options) : base(options)
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
            //Province Many to 1 relationship with Country
            builder.Entity<Province>()
                .HasOne<Country>(a => a.Country)
                .WithMany(d => d.Provinces)
                .HasForeignKey(e => e.CountryId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            //City Many to 1 relationship with Province
            builder.Entity<City>()
                .HasOne<Province>(a => a.Province)
                .WithMany(d => d.Cities)
                .HasForeignKey(e => e.ProvinceId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            //Address 1 to 1 relationship with MailingAddress
            builder.Entity<AddressModel>()
                .HasOne<MailingAddress>(x => x.MailingAddress)
                .WithOne(a => a.AddressModel)
                .HasForeignKey<MailingAddress>(b => b.AddressModelId)
                .IsRequired();
            //Address 1 to 1 relationship with ShippingAddress
            builder.Entity<AddressModel>()
                .HasOne<ShippingAddress>(x => x.ShippingAddress)
                .WithOne(a => a.AddressModel)
                .HasForeignKey<ShippingAddress>(b => b.AddressModelId)
                .IsRequired();
            //MailingAddress Many to 1 relationship with Country
            builder.Entity<MailingAddress>()
                .HasOne<Country>(a => a.Country)
                .WithMany(d => d.MailingAddresses)
                .HasForeignKey(e => e.MailCountry);
            //MailingAddress Many to 1 relationship with Province
            builder.Entity<MailingAddress>()
                .HasOne<Province>(a => a.Province)
                .WithMany(d => d.MailingAddresses)
                .HasForeignKey(e => e.MailProvince);
            //MailingAddress Many to 1 relationship with City
            builder.Entity<MailingAddress>()
                .HasOne<City>(a => a.City)
                .WithMany(d => d.MailingAddresses)
                .HasForeignKey(e => e.MailCity);
            //ShippingAddress Many to 1 relationship with Country
            builder.Entity<ShippingAddress>()
                .HasOne<Country>(a => a.Country)
                .WithMany(d => d.ShippingAddresses)
                .HasForeignKey(e => e.ShipCountry);
            //ShippingAddress Many to 1 relationship with Province
            builder.Entity<ShippingAddress>()
                .HasOne<Province>(a => a.Province)
                .WithMany(d => d.ShippingAddresses)
                .HasForeignKey(e => e.ShipProvince);
            //ShippingAddress Many to 1 relationship with City
            builder.Entity<ShippingAddress>()
                .HasOne<City>(a => a.City)
                .WithMany(d => d.ShippingAddresses)
                .HasForeignKey(e => e.ShipCity);
            //Review composite FK as PK
            builder.Entity<ReviewModel>()
                .HasKey(a => new { a.UserId, a.GameId });
            //Review Many to 1 relationship with User
            builder.Entity<ReviewModel>()
                .HasOne<ApplicationUser>(a => a.User)
                .WithMany(d => d.Reviews)
                .HasForeignKey(e => e.UserId)
                .IsRequired();
            //Review Many to 1 relationship with Game
            builder.Entity<ReviewModel>()
                .HasOne<GameModel>(a => a.Game)
                .WithMany(d => d.Reviews)
                .HasForeignKey(e => e.GameId)
                .IsRequired();
            //Rating composite FK as PK
            builder.Entity<RatingModel>()
                .HasKey(a => new { a.UserId, a.GameId });
            //Rating Many to 1 relationship with User
            builder.Entity<RatingModel>()
                .HasOne<ApplicationUser>(a => a.User)
                .WithMany(d => d.Ratings)
                .HasForeignKey(e => e.UserId)
                .IsRequired();
            //Rating Many to 1 relationship with Game
            builder.Entity<RatingModel>()
                .HasOne<GameModel>(a => a.Game)
                .WithMany(d => d.Ratings)
                .HasForeignKey(e => e.GameId)
                .IsRequired();
            //CreditCard Many to 1 relationship with User
            builder.Entity<CreditCardModel>()
                .HasOne<ApplicationUser>(a => a.User)
                .WithMany(d => d.CreditCards)
                .HasForeignKey(e => e.UserId)
                .IsRequired();
            //Credit Card unique credit card number
            builder.Entity<CreditCardModel>()
                .HasIndex(e => e.CreditCardNumber)
                .IsUnique();

            //FriendList relationship
            builder.Entity<FriendModel>()
                .HasKey(a => new { a.UserId, a.FriendId });
            builder.Entity<FriendModel>()
                .HasOne(a => a.User)
                .WithMany(c => c.UserFriend)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<FriendModel>()
                .HasOne(a => a.Friend)
                .WithMany(c => c.Friend)
                .HasForeignKey(b => b.FriendId)
                .OnDelete(DeleteBehavior.Restrict);

            //Orders Many to 1 relationship with User
            builder.Entity<Orders>()
                .HasOne<ApplicationUser>(a => a.User)
                .WithMany(d => d.Orders)
                .HasForeignKey(e => e.UserId)
                .IsRequired();

            //Orders Many to 1 relationship with Game
            builder.Entity<Orders>()
                .HasOne<GameModel>(a => a.Game)
                .WithMany(d => d.Orders)
                .HasForeignKey(e => e.GameId)
                .IsRequired();

            builder.Entity<ProfileModel>()
                .Property(x => x.Gender)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Entity<GameModel>()
                .Property(x => x.Category)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Entity<GameModel>()
                .Property(x => x.Platform)
                .HasConversion<string>()
                .HasMaxLength(20);

            //Primary Key
            builder.Entity<WishListModel>()
                .HasKey(a => new { a.UserId, a.GameId });

            // Many wishlist to 1 game relationship
            builder.Entity<WishListModel>()
                .HasOne<GameModel>(a => a.Game)
                .WithMany(d => d.Wish)
                .HasForeignKey(e => e.GameId)
                .IsRequired();

            // Many wishlist to 1 user
            builder.Entity<WishListModel>()
                .HasOne<ApplicationUser>(a => a.User)
                .WithMany(d => d.Wish)
                .HasForeignKey(e => e.UserId)
                .IsRequired();

            //EventParticipants Many to 1 relationship with User
            builder.Entity<EventParticipantsModel>()
                .HasOne<ApplicationUser>(a => a.User)
                .WithMany(d => d.EventParticipants)
                .HasForeignKey(e => e.UserId)
                .IsRequired();

            //EventParticipants Many to 1 relationship with Event
            builder.Entity<EventParticipantsModel>()
                .HasOne<EventModel>(a => a.Event)
                .WithMany(d => d.EventParticipants)
                .HasForeignKey(e => e.EventModelId)
                .IsRequired();

            //EventParticipants relationship
            builder.Entity<EventParticipantsModel>()
                .HasKey(a => new { a.UserId, a.EventModelId });
        }

        public DbSet<ProfileModel> Profiles { get; set; }
        public DbSet<GameModel> Games { get; set; }
        public DbSet<PreferencesModel> Preferences { get; set; }
        public DbSet<AddressModel> Address { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Province> Province { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<EventModel> Events { get; set; }
        public DbSet<MailingAddress> MailingAddresses { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<CreditCardModel> CreditCards { get; set; }
        public DbSet<ReviewModel> Reviews { get; set; }
        public DbSet<RatingModel> Ratings { get; set; }
        public DbSet<FriendModel> Friends { get; set; }
        public DbSet<WishListModel> Wish { get; set; }

        public DbSet<Orders> Orders { get; set; }
        public DbSet<EventParticipantsModel> EventParticipants { get; set; }
    }
}
