using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WindowsPet.Models.RepositoryInterface.DatabaseRepositoryInterface;

namespace WindowsPet.Models.Repository
{
    public class FriendRepository : IFriendRepository
    {
        private readonly AppDbContext _context;

        public FriendRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Friend friend)
            => _context.Friends.Add(friend);

        public void Delete(Friend friend)
            => _context.Friends.Remove(friend);

        public Friend? GetByName(string name)
            => _context.Friends.FirstOrDefault(f => f.Name == name);

        public Friend? GetByToken(Guid token)
            => _context.Friends.FirstOrDefault(f => f.Token == token);

        public List<Pet> GetFriendPets(Guid token)
            => _context.Friends.Include(f => f.FriendOwningPets).FirstOrDefault(f => f.Token == token)?.FriendOwningPets?.ToList() ?? new List<Pet>();

        public List<FriendRequest> GetPendingFriendRequests()
            => _context.GetPendingFriendRequest();

        public List<Friend> GetUserFriends()
            => _context.GetUserFriends();

        public void Save()
            => _context.SaveChanges();
    }
}
