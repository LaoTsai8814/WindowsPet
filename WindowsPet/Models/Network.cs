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
using WindowsPet.Models;

namespace WindowsPet.Models
{

    internal class NetworkManager
    {
        private static NetworkManager? _instance;
        public static NetworkManager Instance => _instance ??= new();

        private static TcpClient? TcpClient;

        private static NetworkStream? NetworkStream;

        private StreamReader? _reader;

        private StreamWriter? _writer;

        public event Action<string>? OnMessageReceived;

        public event Action<string>? OnError;

        public event Action? OnDisconnected;

        public event Action<string>? OnSendingDisconnected;

        public static volatile bool OnConnecting = false;





        /// <summary>
        /// Try Connect To Server and Start the Server Respond Handler
        /// </summary>
        /// <returns></returns>
        public async Task CreateAsync()
        {
            #region
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
                
                    
                if(await ConnectToServer())
                {
                    await SendAsync(str);
                    Console.WriteLine("Reconnect And Send Success");
                }
                    

                
                
            });
            #endregion
            HandleFromServer.Instance.ServerRespondHandler();
            await Instance.ConnectToServer();
        }





        /// <summary>
        /// Try Connect To Server
        /// Set Reader and Writer Stream
        /// Open A Thread For Receive Server Respond Message
        ///</summary>
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
            Thread ReceiveThread = new Thread(async () =>
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
            ReceiveThread.IsBackground = true;
            ReceiveThread.Start();
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
        public async Task ReceiveAsync()
        {
            if (_reader == null || OnMessageReceived == null || OnDisconnected == null)
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
                    OnMessageReceived.Invoke(line);
                }
            }
            catch (Exception ex)
            {
                // 可以觸發 OnError 事件或 log
                OnError?.Invoke(ex.Message);
                Console.WriteLine($"接收錯誤: {ex.Message}");
            }

        }
        
        
        NetworkManager() { }

    }



    /// <summary>
    /// Json Serialize and Deserialize Include Serialize And Send
    /// </summary>
    internal class JsonSerialize
    {
        public static string SerializeJson(Type type)
        {
            return JsonConvert.SerializeObject(type);

        }
        public static async Task SerializeAndSendJson<T>(T obj)
        {
            await NetworkManager.Instance.SendAsync(JsonConvert.SerializeObject(obj));

        }
        public static T? DeserializeJson<T>(string json)
        {
            // Deserialize the JSON string into an object of type T

            return JsonConvert.DeserializeObject<T>(json) ;
        }

    }




    /// <summary>
    ///  Get the Main Server IP and port
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



