using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using WindowsPet.Models;
using WindowsPet.Models.RepositoryInterface.Network;

namespace WindowsPet.Models
{
    public class HandleFromServer
    {
        private readonly IServiceProvider _provider;

        public Action<string>? OnReceiveMessage;

        public HandleFromServer(IServiceProvider provider)
        {
            _provider = provider;
            OnReceiveMessage += OnReceiveServerRespond;
        }

        private void OnReceiveServerRespond(string receive)
        {
            var state = JsonSerialize.DeserializeJson<ServerRespondStatus>(receive);
            if (state == null) return;

            // 找出對應的型別
            Type? type = Type.GetType("WindowsPet.Models." + state.RequestName);

            // 反序列化 RespondParameter 成真正型別
            if (type != null && state.RespondParameter is JObject jObj)
            {
                object? realParameter = jObj.ToObject(type);
                state.RespondParameter = realParameter;
            }

            if (Application.Current != null && Application.Current.Dispatcher != null)
            {
                Application.Current.Dispatcher.Invoke(() => Dispatch(state));
            }
            else
            {
                Dispatch(state);
            }
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
}
