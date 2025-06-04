using Microsoft.Extensions.DependencyInjection;
using WindowsPet.Models.RepositoryInterface.Network;
using WindowsPet.Models;
using System.Windows;
using WindowsPet.VM.TabsVM;
using WindowsPet;
using Newtonsoft.Json.Linq;

internal class HandleFromServer
{
    
    private readonly IServiceProvider _provider;
    

    public HandleFromServer(IServiceProvider provider)
    {
        _provider = provider;
        OnReceiveMessage += OnReceiveServerRespond;
    }
    public Action<string> OnReceiveMessage;
    public void ServerRespondHandler()
    {
        if(App.ServiceProvider.GetService<NetworkManager>() == null)
        {
            Console.WriteLine("NetworkManager is null");
            return;
        }
        App.ServiceProvider.GetService<NetworkManager>()!.OnMessageReceived+=OnReceiveServerRespond;
    }

    private void OnReceiveServerRespond(string receive)
    {
        var state = JsonSerialize.DeserializeJson<ServerRespondStatus>(receive);
        // 找出對應的型別
        Type? type = Type.GetType("WindowsPet.Models." + state.RequestName);

        // 反序列化 RespondParameter 成真正型別
        if (type != null && state.RespondParameter is JObject jObj)
        {
            object realParameter = jObj.ToObject(type);
            state.RespondParameter = realParameter;
        }
        Application.Current.Dispatcher.Invoke(() => Dispatch(state));
    }

    private void Dispatch(object? obj)
    {
        if (obj == null) return;

        var type = obj.GetType();
        var handlerType = typeof(INetworkRepository<>).MakeGenericType(type);
        var handler = _provider.GetService(handlerType);

        if (handler != null)
        {
            var method = handlerType.GetMethod("Handler");
            method?.Invoke(handler, new[] { obj });
        }
        else
        {
            Console.WriteLine($"未找到 {type.Name} 對應的處理者");
        }
    }

   
}
