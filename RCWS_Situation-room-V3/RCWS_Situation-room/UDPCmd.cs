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
        private UdpClient UDP_CLIENT;
        private IPEndPoint ENDPOINT;
        public event Action<byte[]> VideoReceived;

        private int frameCount = 0;
        private Stopwatch stopwatch = new Stopwatch();

        public UDPCmd(FormMain main)
        {
            _formmain = main;
        }

        public void StartReceiving(string ip, int port)
        {
            UDP_CLIENT = new UdpClient(port);
            ENDPOINT = new IPEndPoint(IPAddress.Parse(ip), port);
            stopwatch.Start();
            BeginReceive();
        }

        private void BeginReceive()
        {
            UDP_CLIENT.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                byte[] receivedData = UDP_CLIENT.EndReceive(ar, ref ENDPOINT);

                int totalPackets = receivedData[0];
                int packetNumber = receivedData[1];

                List<byte> allData = new List<byte>();
                allData.AddRange(receivedData.Skip(2));

                for (int i = 1; i < totalPackets; i++)
                {
                    byte[] dgram = UDP_CLIENT.Receive(ref ENDPOINT);
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