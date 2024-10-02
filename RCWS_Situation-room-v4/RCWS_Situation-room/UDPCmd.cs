using System.Linq;
using System.Net.Sockets;
using System.Net;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace RCWS_Situation_room
{    
    public class UDPCmd
    {
        public FormMain _formmain;
        private UdpClient _udpClient;
        private IPEndPoint _endPoint;
        public event Action<byte[]> VideoReceived;

        private int frameCount = 0;
        private readonly Stopwatch stopwatch = new Stopwatch();

        public void StartReceiving(string ip, int port)
        {
            _udpClient = new UdpClient(port);
            _endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            stopwatch.Start();
            BeginReceive();
        }

        private void BeginReceive()
        {
            _udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                byte[] receivedData = _udpClient.EndReceive(ar, ref _endPoint);

                int totalPackets = receivedData[0];
                int packetNumber = receivedData[1];

                List<byte> allData = new List<byte>();
                allData.AddRange(receivedData.Skip(2));

                for (int i = 1; i < totalPackets; i++)
                {
                    byte[] dgram = _udpClient.Receive(ref _endPoint);
                    allData.AddRange(dgram.Skip(2));
                }

                if (allData.Count > 0)
                {
                    VideoReceived?.Invoke(allData.ToArray());
                    frameCount++;
                }

                if (stopwatch.ElapsedMilliseconds >= 1000)
                {
                    double fps = frameCount / (stopwatch.ElapsedMilliseconds / 1000.0);
                    //MessageBox.Show($"FPS: {fps:F2}");
                    _formmain.UpdateUI(() => _formmain.SendDisplay($"FPS: {fps:F2}"));
                    frameCount = 0;
                    stopwatch.Restart();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            finally
            {
                BeginReceive();
            }
        }
    }
}