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
    internal class AppDbContext : DbContext
    {
        private static AppDbContext? _appdbcontext;

        public static AppDbContext Instance => _appdbcontext ??= new();

        public DbSet<PersonalData> Users { get; set; }

        /// <summary>
        /// The pets that the user has purchased.
        /// </summary>
        public DbSet<Pet> Pets { get; set; }

        public DbSet<Friend> Friends { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=account.db");

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
        }
        public void ConnectToDB()
        {
            // Create the database if it doesn't exist          
            Instance.Database.EnsureCreated();
        }
        public void AddUser(PersonalData data)
        {
            // Add a new user to the database

            try
            {
                var user = Users.FirstOrDefault(u => u.Email == data.Email);
                if (user != null)
                {
                    // User already exists, handle accordingly
                    CurrentUser.Token = user.Token;
                    return;
                }
                Users.Add(data);
                CurrentUser.Token = data.Token;
                CurrentUser.Credit = data.Credit;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
        public void SaveChangesToDB()
        {
            // Save changes to the database
            try
            {
                Instance.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public string GetUserToken(string email)
        {
            // Get the user token from the database
            try
            {
                var user = Users.FirstOrDefault(u => u.Email == email);
                if (user != null && user.Token != null)
                {
                    return user.Token;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }
        public void DeletePopularPet()
        {
            // Delete a pet from the database
            try
            {
                var pet = Pets.FirstOrDefault(u => u.IsPopular == true);
                if (pet != null)
                {
                    Pets.Remove(pet);
                    SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void AddPetListToUser(string token, List<Pet> pets)
        {
            // Add a pet list to the user
            if (pets == null)
            {
                return;
            }
            try
            {
                var user = Users.Include(u => u.UserPets).FirstOrDefault(u => u.Token == token);
                if (user.UserPets == null)
                    user.UserPets = new List<Pet>();
                if (user != null)
                {
                    foreach (var pet in pets)
                    {
                        var trackedPet = Pets.FirstOrDefault(p => p.Id == pet.Id);
                        if (trackedPet != null)
                        {
                            if (!user.UserPets.Any(u => u.Id == trackedPet.Id))
                            {
                                user.UserPets?.Add(trackedPet);
                            }

                        }
                        else
                        {
                            // 如果是新寵物（未存在 DB），可以選擇先加入 Pets 資料表
                            Pets.Add(pet);
                            user.UserPets?.Add(pet);
                        }
                    }
                }
                else
                {

                }
                SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void AddPopularPetToTable(List<Pet>? petList)
        {
            if (petList == null || petList.Count == 0) return;

            try
            {
                foreach (var pet in petList)
                {
                    // 檢查資料庫中是否已存在該寵物
                    var existingPet = Pets.FirstOrDefault(p => p.Id == pet.Id);

                    if (existingPet == null)
                    {
                        Pets.Add(pet); // 加入到資料庫的 DbSet<Pet>
                    }

                }

                SaveChanges(); // 實際寫入資料庫
            }
            catch (Exception ex)
            {
                Console.WriteLine($"新增熱門寵物時失敗: {ex.Message}");
            }
        }


        public bool IsPetPurchased(string token, int petId)
        {
            // Check if the pet is purchased by the user
            try
            {
                var user = Users.FirstOrDefault(u => u.Token == token);
                if (user != null)
                {
                    return user.UserPets?.Any(p => p.Id == petId) ?? false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

        }
        public bool IsPetPurchased(string? token, string? petname)
        {
            // Check if the pet is purchased by the user
            try
            {
                var user = Users.FirstOrDefault(u => u.Token == token);
                if (user != null)
                {
                    return user.UserPets?.Any(p => p.Name == petname) ?? false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public Pet GetPet(string? Petname)
        {             // Get the pet from the database
            try
            {
                var pet = Pets.FirstOrDefault(u => u.Name == Petname);
                if (pet != null)
                {
                    return pet;
                }
                else
                {
                    return null!;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null!;
            }
        }
        public Pet? GetPopularPet(string? Petname)
        {             // Get the pet from the database
            try
            {
                var pet = Pets.FirstOrDefault(u => u.Name == Petname && u.IsPopular == true);
                if (pet != null)
                {
                    return pet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public Pet? GetPopularPet(int PetId)
        {             // Get the pet from the database
            try
            {
                var pet = Pets.FirstOrDefault(u => u.Id == PetId && u.IsPopular == true);
                if (pet != null)
                {
                    return pet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        internal void VerifyPetPrice(int petId, decimal price)
        {
            Pet? pet = Pets.FirstOrDefault(p => p.Id == petId);
            if (pet == null)
            {
                // Handle the case where the pet is not found
                return;
            }
            pet.Price = price;
            SaveChanges();

            //throw new NotImplementedException();
        }

        internal void AddPetToUser(string? token, int petId)
        {
            try
            {
                // Add a pet to the user
                var user = Users.FirstOrDefault(u => u.Token == token);
                if (user == null)
                {
                    // Handle the case where the user is not found
                    return;
                }
                Pet? pet = Pets.FirstOrDefault(p => p.Id == petId);
                if (pet != null)
                    user.UserPets!.Add(pet!);
                SaveChanges();
                ViewModelManager.Instance.GetViewModel<HomeTabVM>(TabManager.Instance.GetTabObject<HomeTab>())
                    .MyFavoritePets.Add(new UIPets(pet.Name, pet.ImagePath));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            //throw new NotImplementedException();
        }

        internal void UpdateUserCredit(string? token, decimal credit)
        {
            var user = Users.FirstOrDefault(u => u.Token == token);

            if (user != null)
            {
                user.Credit = credit;
            }
            SaveChanges();

            //throw new NotImplementedException();
        }
        public bool IsPetOwnByUser(int id)
        {
            var pet = Users.Include(u => u.UserPets).FirstOrDefault(u => u.Token == CurrentUser.Token)!.UserPets?.FirstOrDefault(p => p.Id == id);
            if (pet == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool IsPetOwnByUser(string? name)
        {
            var pet = Users.Include(u => u.UserPets).FirstOrDefault(u => u.Token == CurrentUser.Token)!.UserPets?.FirstOrDefault(p => p.Name == name);
            if (pet == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        internal void AddPendingFriendToUser(List<Friend> PendingFriendRequest)
        {
            var user = Users.Include(u => u.PendingFriendRequest).FirstOrDefault(u => u.Token == CurrentUser.Token);
            if (user == null)
                return;

            foreach (var pendingrequest in PendingFriendRequest)
            {
                if (user.Token != pendingrequest.UserId)
                {
                    user.PendingFriendRequest.Add(pendingrequest);
                }
            }
            //throw new NotImplementedException();
        }

        internal void AddFriendToUser(List<Friend> friendList)
        {
            var user = Users.Include(u => u.Friend).FirstOrDefault(u => u.Token == CurrentUser.Token);
            if (user == null)
                return;

            foreach (var friend in friendList)
            {
                if (user.Token != friend.UserId)
                {
                    user.PendingFriendRequest.Add(friend);
                }
            }
            //throw new NotImplementedException();


            //throw new NotImplementedException();
        }
    }
}
