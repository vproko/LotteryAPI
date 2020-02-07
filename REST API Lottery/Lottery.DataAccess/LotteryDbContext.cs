using Lottery.DomainClasses.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Lottery.DataAccess
{
    public class LotteryDbContext : DbContext
    {
        public LotteryDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Draw> Drawings { get; set; }
        public DbSet<Winner> Winners { get; set; }
        public DbSet<Prize> Prizes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //USER

            //User's primary key
            builder.Entity<User>()
                .HasKey(u => u.UserId);

            //User's identity
            builder.Entity<User>()
                .Property(u => u.UserId)
                .ValueGeneratedOnAdd();

            //User's required properties
            builder.Entity<User>()
                .Property(u => u.FirstName)
                .IsRequired();

            builder.Entity<User>()
                .Property(u => u.LastName)
                .IsRequired();

            builder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired();

            builder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired();

            builder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            builder.Entity<User>()
                .Property(u => u.Joined)
                .IsRequired();

            builder.Entity<User>()
                .Property(u => u.Role)
                .IsRequired();

            //User's relations
            builder.Entity<User>()
                .HasMany(u => u.Tickets)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //WINNER

            //Winner's primary key
            builder.Entity<Winner>()
                .HasKey(w => w.WinnerId);

            //Winner's identity
            builder.Entity<Winner>()
                .Property(w => w.WinnerId)
                .ValueGeneratedOnAdd();

            //Winner's required properties
            builder.Entity<Winner>()
                .Property(w => w.NumberOfHits)
                .IsRequired();

            builder.Entity<Winner>()
                .Property(w => w.PrizeId)
                .IsRequired();

            builder.Entity<Winner>()
                .Property(w => w.WinningNumbers)
                .IsRequired();

            builder.Entity<Winner>()
                .Property(w => w.UserId)
                .IsRequired();

            builder.Entity<Winner>()
                .Property(w => w.TicketId)
                .IsRequired();

            // Winners relations
            builder.Entity<Winner>()
                .HasOne(w => w.Prize)
                .WithMany(p => p.Winners)
                .HasForeignKey(w => w.PrizeId);

            //TICKET

            //Ticket's primary key
            builder.Entity<Ticket>()
                .HasKey(t => t.TicketId);

            //Ticket's identity
            builder.Entity<Ticket>()
                .Property(t => t.TicketId)
                .ValueGeneratedOnAdd();

            //Ticket's required properties
            builder.Entity<Ticket>()
                .Property(t => t.CreateDate)
                .IsRequired();

            builder.Entity<Ticket>()
                .Property(t => t.Numbers)
                .IsRequired();

            builder.Entity<Ticket>()
                .Property(t => t.UserId)
                .IsRequired();

            builder.Entity<Ticket>()
                .Property(t => t.SessionId)
                .IsRequired();

            //Ticket's relations
            builder.Entity<Ticket>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tickets);

            //SESSION

            //Session's primary key
            builder.Entity<Session>()
                .HasKey(s => s.SessionId);

            //Session's Identity
            builder.Entity<Session>()
                .Property(s => s.SessionId)
                .ValueGeneratedOnAdd();

            //Session's required properties
            builder.Entity<Session>()
                .Property(s => s.StartDate)
                .IsRequired();

            builder.Entity<Session>()
                .Property(s => s.EndDate)
                .IsRequired();

            //Session's relations
            builder.Entity<Session>()
                .HasMany(s => s.Tickets)
                .WithOne(t => t.Session)
                .HasForeignKey(t => t.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Session>()
                .HasMany(s => s.Winners)
                .WithOne(w => w.Session)
                .HasForeignKey(w => w.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            //DRAW

            //Draw's primary key
            builder.Entity<Draw>()
                .HasKey(d => d.DrawId);

            //Draw's identity
            builder.Entity<Draw>()
                .Property(d => d.DrawId)
                .ValueGeneratedOnAdd();

            //Draw's required properties
            builder.Entity<Draw>()
                .Property(d => d.Date)
                .IsRequired();

            builder.Entity<Draw>()
                .Property(d => d.DrawnNumbers)
                .IsRequired();

            builder.Entity<Draw>()
                .Property(d => d.SessionId)
                .IsRequired();

            //Draw's relations
            builder.Entity<Draw>()
                .HasOne(d => d.Session)
                .WithOne(s => s.Draw)
                .HasForeignKey<Draw>(d => d.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            //PRIZE

            //Prize's primary key
            builder.Entity<Prize>()
                .HasKey(p => p.PrizeId);

            //Prize's identity
            builder.Entity<Prize>()
                .Property(p => p.PrizeId)
                .ValueGeneratedOnAdd();

            //Prize's required properties
            builder.Entity<Prize>()
                .Property(p => p.Name)
                .IsRequired();

            builder.Entity<Prize>()
                .Property(p => p.NumberOfHits)
                .IsRequired();

            //DATA SEED

            var md5 = new MD5CryptoServiceProvider();
            var md5data = md5.ComputeHash(Encoding.ASCII.GetBytes("admin123"));
            var hashedPassword = Encoding.ASCII.GetString(md5data);

            builder.Entity<User>().HasData(
                    new User()
                    {
                        UserId = Guid.NewGuid(),
                        FirstName = "John",
                        LastName = "Doe",
                        Username = "jdoe",
                        Password = hashedPassword,
                        Email = "jdoe@mail.com",
                        Joined = DateTime.UtcNow,
                        Role = "Admin"
                    }
                );

            builder.Entity<Prize>().HasData(
                    new Prize()
                    {
                        PrizeId = Guid.NewGuid(),
                        Name = "50$ Gift Card",
                        NumberOfHits = 3,
                    },
                    new Prize()
                    {
                        PrizeId = Guid.NewGuid(),
                        Name = "100$ Gift Card",
                        NumberOfHits = 4,
                    },
                    new Prize()
                    {
                        PrizeId = Guid.NewGuid(),
                        Name = "TV",
                        NumberOfHits = 5,
                    },
                    new Prize()
                    {
                        PrizeId = Guid.NewGuid(),
                        Name = "Car",
                        NumberOfHits = 6,
                    },
                    new Prize()
                    {
                        PrizeId = Guid.NewGuid(),
                        Name = "(JackPot) - 100,000$",
                        NumberOfHits = 7,
                    }
                );
        }
    }
}
