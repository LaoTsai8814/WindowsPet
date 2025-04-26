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

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=account.db");

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
                if (user != null)
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






    }
}
