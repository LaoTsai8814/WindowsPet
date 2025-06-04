using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPet.Models.RepositoryInterface.Network
{
    public interface INetworkRepository<T>
    {
        void OnSuccessStatus();

        void OnFailedStatus();
        void Handler(T Command);
    }
}
