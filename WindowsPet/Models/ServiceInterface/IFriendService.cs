using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPet.Models.ServiceInterface
{
    public interface IFriendService
    {

        void AddFriendListToUser(Guid token, List<Friend> friend);

        void AddFriendRequestListToUser(Guid token, List<FriendRequest> friendRequest);

        void AddFriendToUser(Guid token, Friend friend);
        
        void AddFriendRequestToUser(Guid UserToken, FriendRequest FriendRequest);


    }
}
