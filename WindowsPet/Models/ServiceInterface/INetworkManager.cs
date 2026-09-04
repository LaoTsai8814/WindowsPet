using System;
using System.Threading.Tasks;

namespace WindowsPet.Models.ServiceInterface
{
    public interface INetworkManager
    {
        event Action<string> OnMessageReceived;
        event Action<string>? OnError;
        event Action? OnDisconnected;
        event Action<string>? OnSendingDisconnected;
        Task CreateAsync();
        Task SendAsync(string message);
        Task SendJsonAsync<T>(T obj);
    }
}
