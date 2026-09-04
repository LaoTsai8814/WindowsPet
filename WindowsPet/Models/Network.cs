using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsPet.Models.ServiceInterface;

namespace WindowsPet.Models
{
    public class NetworkManager : INetworkManager
    {
        private readonly HandleFromServer _handleFromServer;

        private static TcpClient? TcpClient;
        private static NetworkStream? NetworkStream;
        private StreamReader? _reader;
        private StreamWriter? _writer;

        public event Action<string>? OnMessageReceived;
        public event Action<string>? OnError;
        public event Action? OnDisconnected;
        public event Action<string>? OnSendingDisconnected;
        public static volatile bool OnConnecting = false;

        public NetworkManager(HandleFromServer handleFromServer)
        {
            _handleFromServer = handleFromServer;
            Task.Run(async () =>
            {
                await CreateAsync();
            });
        }

        /// <summary>
        /// Try Connect To Server and Start the Server Respond Handler
        /// </summary>
        public async Task CreateAsync()
        {
            OnDisconnected += (async () =>
            {
                if (!OnConnecting)
                {
                    await ConnectToServer();
                    OnConnecting = true;
                }
            });
            OnSendingDisconnected += (async (string str) =>
            {
                if (await ConnectToServer())
                {
                    await SendAsync(str);
                    Console.WriteLine("Reconnect And Send Success");
                }
            });

            await ConnectToServer();
        }

        /// <summary>
        /// Try Connect To Server
        /// Set Reader and Writer Stream
        /// Open A Thread For Receive Server Respond Message
        /// </summary>
        private async Task<bool> ConnectToServer()
        {
            try
            {
                TcpClient = new TcpClient();
                await TcpClient.ConnectAsync(NetworkSetup.GetServerIP(), NetworkSetup.GetServerPort());
                NetworkStream = TcpClient.GetStream();
                Console.WriteLine("ServerConnected");
            }
            catch (SocketException ex)
            {
                OnError?.Invoke($"SocketException: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Exception: {ex.Message}");
                return false;
            }
            finally
            {
                OnConnecting = false;
            }

            _reader = new StreamReader(NetworkStream, Encoding.UTF8);
            _writer = new StreamWriter(NetworkStream, Encoding.UTF8) { AutoFlush = true };

            Thread receiveThread = new Thread(async () =>
            {
                try
                {
                    await ReceiveAsync();
                }
                catch (Exception ex)
                {
                    OnError?.Invoke($"ReceiveThread Exception: {ex.Message}");
                }
            });
            receiveThread.IsBackground = true;
            receiveThread.Start();
            return true;
        }

        public async Task SendAsync(string message)
        {
            try
            {
                while (TcpClient == null || !TcpClient.Connected)
                {
                    OnSendingDisconnected?.Invoke(message);
                    OnError?.Invoke("TCP Client is not connected. Reconnecting");
                    return;
                }
                if (_writer != null)
                {
                    await _writer.WriteLineAsync(message);
                }
            }
            catch (Exception ex)
            {
                OnSendingDisconnected?.Invoke(message);
                OnError?.Invoke($"Error Message:{ex}");
            }
        }

        public async Task SendJsonAsync<T>(T obj)
        {
            await SendAsync(JsonConvert.SerializeObject(obj));
        }

        public async Task ReceiveAsync()
        {
            if (_reader == null || OnDisconnected == null)
                return;
            try
            {
                while (true)
                {
                    var line = await _reader.ReadLineAsync();
                    if (line == null)
                    {
                        OnDisconnected.Invoke();
                        break; // 連線斷了
                    }
                    Console.WriteLine(line);
                    OnMessageReceived?.Invoke(line);
                    _handleFromServer.OnReceiveMessage?.Invoke(line);
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
                Console.WriteLine($"接收錯誤: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Json Serialize and Deserialize Utilities
    /// </summary>
    public static class JsonSerialize
    {
        public static string SerializeJson(Type type)
        {
            return JsonConvert.SerializeObject(type);
        }

        public static string SerializeJson<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T? DeserializeJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }

    /// <summary>
    /// Get the Main Server IP and port
    /// </summary>
    internal class NetworkSetup
    {
        static readonly IPAddress ipaddr = new IPAddress(new byte[] { 192, 168, 0, 104 });
        static readonly int port = 8144;

        internal protected static IPAddress GetServerIP()
        {
            return ipaddr;
        }

        internal protected static int GetServerPort()
        {
            return port;
        }
    }
}
