using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace WindowsPet.Models
{
    internal class AppDbContext : DbContext
    {
        private static AppDbContext _appdbcontext;

        public static AppDbContext Instance => _appdbcontext ??= new();

        public DbSet<PersonalData> Users { get; set; }

        public DbSet<Pet> Pets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=account.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonalData>()
                .HasMany(u => u.UserPets)
                .WithMany(p => p.Owner)
                .UsingEntity(j => j.ToTable("UserPets"));
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
                Users.Add(data);
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
                if (user != null && user.Token!=null)
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
        public void AddPetListToUser(string token,List<Pet> pets)
        {
            // Add a pet list to the user
            if (pets == null)
            {
                return;
            }
            try
            {
                var user = Users.FirstOrDefault(u=>u.Token == token);
                if (user != null)
                {
                    foreach (var pet in pets)
                    {
                        var trackedPet = Pets.FirstOrDefault(p => p.Id == pet.Id);
                        if (trackedPet != null)
                        {
                            user.UserPets?.Add(trackedPet);
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
                SaveChangesToDB();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }






        }
    }
}
