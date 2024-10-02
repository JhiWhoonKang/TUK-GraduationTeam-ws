using System;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using RCWS_Situation_room.Data;

namespace RCWS_Situation_room.Services
{  
    public class TCPService:IDisposable
    {
        private readonly TcpClient _tcpClient;
        private NetworkStream _networkStream;
        private StreamReader _streamReader;
        private StreamWriter _streamWriter;
        private readonly CancellationTokenSource _cts;

        public event Action<string> OnStatusUpdate;
        public event Action<byte[]> OnDataReceived;
        public event Action<string> OnError;

        public TCPService()
        {
            _tcpClient = new TcpClient();
            _cts = new CancellationTokenSource();
        }

        public async Task ConnectAsync(string serverIp, int port)
        {
            try
            {
                OnStatusUpdate?.Invoke("Connecting...");
                await _tcpClient.ConnectAsync(serverIp, port);
                _networkStream = _tcpClient.GetStream();
                _streamReader = new StreamReader(_networkStream);
                _streamWriter = new StreamWriter(_networkStream) { AutoFlush = true };

                OnStatusUpdate?.Invoke("Server Connected");
                StartReceiving(_cts.Token);
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Connection Error: {ex.Message}");
            }
        }

        private async void StartReceiving(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    byte[] buffer = new byte[Marshal.SizeOf(typeof(Packet.RECEIVED_PACKET))];
                    int bytesRead = await _networkStream.ReadAsync(buffer, 0, buffer.Length, token);
                    if (bytesRead == 0)
                        throw new Exception("Disconnected from server");

                    OnDataReceived?.Invoke(buffer);
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Receive Error: {ex.Message}");
            }
        }

        public async Task SendAsync<T>(T data) where T : struct
        {
            try
            {
                byte[] bytes = TCPCmd.StructToBytes(data);
                await _networkStream.WriteAsync(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Send Error: {ex.Message}");
            }
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
