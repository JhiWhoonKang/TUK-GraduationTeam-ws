using System;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using RCWS_Situation_room.Data;
using RCWS_Situation_room.Services;

namespace RCWS_Situation_room.Services
{
    public class TCPService : IDisposable
    {
        private readonly TcpClient _tcpClient;
        private NetworkStream _networkStream;
        private readonly CancellationTokenSource _cts;

        public event Action<string> OnStatusUpdate;
        public event Action<byte[]> OnDataReceived;
        public event Action<string> OnError;
        public event Action OnConnected;

        private bool isError = false;

        public TCPService()
        {
            _tcpClient = new TcpClient();
            _cts = new CancellationTokenSource();
        }

        public async Task TCP_Connect_Async(string serverIp, int port)
        {
            try
            {
                ConnectMsg("Connecting...");
                await ConnectToServerAsync(serverIp, port);

                InitializeStreams();
                ConnectMsg("Server Connected");
                Notify(OnConnected);
                _ = StartReceivingAsync(_cts.Token);
            }
            catch (Exception ex)
            {
                Notify(OnError, ex.Message);
            }
        }

        private async Task ConnectToServerAsync(string serverIp, int port)
        {
            await _tcpClient.ConnectAsync(serverIp, port);
        }

        private void InitializeStreams()
        {
            _networkStream = _tcpClient.GetStream();
        }

        private async Task StartReceivingAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var buffer = new byte[28];
                    int bytesRead = await _networkStream.ReadAsync(buffer, 0, buffer.Length, token);
                    if (bytesRead == 0)
                    {
                        throw new Exception("Disconnected from server");
                    }
                    Notify(OnDataReceived, buffer);
                }
            }
            catch (Exception ex)
            {
                if (!token.IsCancellationRequested)
                {
                    Notify(OnError, ex.Message);
                }
            }
        }

        public async Task SendAsync<T>(T data) where T : struct
        {
            try
            {
                byte[] bytes = TCPCmd.StructToBytes(data);
                await _networkStream.WriteAsync(bytes, 0, bytes.Length);
                await _networkStream.FlushAsync();
            }
            catch (Exception ex)
            {
                Notify(OnError, ex.Message);
            }
        }

        private void ConnectMsg(string msg)
        {
            Notify(OnStatusUpdate, msg);
        }

        private void Notify(Action action)
        {
            action?.Invoke();
        }

        private void Notify<T>(Action<T> action, T message)
        {
            action?.Invoke(message);
        }

        public void Dispose()
        {
            _cts.Cancel();
            _networkStream?.Close();
            _tcpClient?.Close();
            _cts.Dispose();
        }
    }
}