using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RCWS_Situation_room
{
    public class Network_TCP_UDP
    {
        private readonly UdpClient _udpClient;
        private IPEndPoint _endPoint;
        public event Action<byte[]> DataReceived;

        public Network_TCP_UDP(string ip, int port)
        {
            _udpClient = new UdpClient(port);
            _endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            //Begin_Receive();
        }

        public void Begin_Receive()
        {
            _udpClient.BeginReceive(ReceiveCallback, null);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                var receivedData = _udpClient.EndReceive(ar, ref _endPoint);

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
                    DataReceived?.Invoke(allData.ToArray());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("데이터 수신 오류: " + ex.Message);
            }
            finally
            {
                Begin_Receive();
            }
        }

        public void Close()
        {
            _udpClient?.Close();
        }
    }
}
