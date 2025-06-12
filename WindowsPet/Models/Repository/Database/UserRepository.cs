using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WindowsPet.Models.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Add(PersonalData user)
            => _context.Users.Add(user);

        public PersonalData? GetByEmail(string email)
            => _context.Users.FirstOrDefault(u => u.Email == email);

        public PersonalData? GetByToken(Guid? token)
            => _context.Users.FirstOrDefault(u => u.Token == token);

        public void Save()
            => _context.SaveChanges();
    }
}
