using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsPet.Models.ServiceInterface;

namespace WindowsPet.Models.Service
{
    public class FriendService : IFriendService
    {
        public void AddFriendListToUser(Guid token, List<Friend> friend)
        {
            throw new NotImplementedException();
        }

        public void AddFriendRequestListToUser(Guid token, List<FriendRequest> friendRequest)
        {
            throw new NotImplementedException();
        }

        public void AddFriendRequestToUser(Guid token, FriendRequest friendRequest)
        {
            throw new NotImplementedException();
        }

        public void AddFriendToUser(Guid token, Friend friend)
        {
            throw new NotImplementedException();
        }
    }
}
