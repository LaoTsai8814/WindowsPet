using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using WindowsPet.VM.TabsVM;
using WindowsPet.Views.Tabs;

namespace WindowsPet.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<PersonalData> Users { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<PetCategories> Categories { get; set; }

        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlite("Data Source=account.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonalData>()
                .HasMany(u => u.UserPets)
                .WithMany(p => p.Owner)
                .UsingEntity(j => j.ToTable("UserPets"));

            modelBuilder.Entity<Friend>()
                .HasKey(f => new { f.UserId, f.FriendId }); // Composite Key

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.User)
                .WithMany(p => p.Friend)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.FriendUser)
                .WithMany(p => p.FriendOf)
                .HasForeignKey(f => f.FriendId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friend>()
                .ToTable("UserFriend"); // 自訂表名

            modelBuilder.Entity<PersonalData>()
                .HasMany(u => u.ReceivedFriendRequests)
                .WithMany(f => f.RequestUser)
                .UsingEntity(j => j.ToTable("FriendRequestsTable"));

            modelBuilder.Entity<Pet>()
                .HasMany(p => p.PetCategories)
                .WithMany(c => c.Pets)
                .UsingEntity(j => j.ToTable("PetCategories"));
        }

        public void ConnectToDB()
        {
            Database.EnsureCreated();
        }

        public void AddPendingFriendToUser(List<FriendRequest> pendingFriendRequest)
        {
            try
            {
                var user = Users.Include(u => u.ReceivedFriendRequests).FirstOrDefault(u => u.Token == CurrentUser.Token);
                if (user == null)
                    return;

                foreach (var pendingrequest in pendingFriendRequest)
                {
                    if (FriendRequests.Any(u => u.Id == pendingrequest.Id))
                        continue;
                    FriendRequests.Add(pendingrequest);
                    if (user.Token != pendingrequest.FromUserId)
                    {
                        user.ReceivedFriendRequests.Add(pendingrequest);
                    }
                    SaveChanges();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
            }
        }

        public void AddFriendToUser(List<Friend> friendList)
        {
            var user = Users.Include(u => u.Friend).FirstOrDefault(u => u.Token == CurrentUser.Token);
            if (user == null)
                return;

            foreach (var friend in friendList)
            {
                if (user.Token != friend.UserId)
                {
                    user.Friend.Add(friend);
                }
            }
            SaveChanges();
        }

        public List<FriendRequest> GetPendingFriendRequest()
        {
            var user = Users.Include(u => u.ReceivedFriendRequests).FirstOrDefault(u => u.Token == CurrentUser.Token);
            if (user == null)
                return new List<FriendRequest>();
            else
            {
                return user.ReceivedFriendRequests.ToList();
            }
        }

        public List<Friend> GetUserFriends()
        {
            var user = Users.Include(u => u.Friend).FirstOrDefault(u => u.Token == CurrentUser.Token);
            if (user == null)
                return new List<Friend>();
            else
            {
                return user.Friend.ToList();
            }
        }

        public void RemovePendingFriendRequest(Guid fromUserId)
        {
            try
            {
                var friend = FriendRequests.FirstOrDefault(u => u.FromUserId == fromUserId);
                if (friend != null)
                {
                    FriendRequests.Remove(friend);
                    SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void AddFriendToTable(Friend friend)
        {
            try
            {
                var f = Users.Include(u => u.Friend).FirstOrDefault(u => u.Token == CurrentUser.Token);
                if (f != null && !f.Friend.Any(u => u.Token == friend.Token))
                {
                    f.Friend.Add(friend);
                    SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
