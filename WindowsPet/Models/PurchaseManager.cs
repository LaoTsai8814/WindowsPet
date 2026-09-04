using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WindowsPet.Models.ServiceInterface;

namespace WindowsPet.Models
{
    public class PurchaseManager : IPurchaseManager
    {
        private readonly INetworkManager? _networkManager;

        public PurchaseManager(INetworkManager? networkManager = null)
        {
            _networkManager = networkManager;
        }
    }
}
