using System.Linq;
using System.Net.Sockets;
using System.Net;
using System;

namespace RCWS_Situation_room
{
    public class UDPCmd
    {
        private UdpClient UDP_CLIENT;
        private IPEndPoint ENDPOINT;
        public event Action<byte[]> VideoReceived;

        public void StartReceiving(string ip, int port)
        {
            UDP_CLIENT = new UdpClient(port);
            ENDPOINT = new IPEndPoint(IPAddress.Parse(ip), port);

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

                if (receivedData.Length < 65000)
                {
                    VideoReceived?.Invoke(receivedData);
                }
                else
                {
                    byte[] dgram = UDP_CLIENT.Receive(ref ENDPOINT);
                    if (dgram.Length < 65000)
                    {
                        VideoReceived?.Invoke(receivedData.Concat(dgram).ToArray());
                    }
                    else
                    {
                        byte[] dgram_ = receivedData.Concat(dgram).ToArray();
                        byte[] dgram__ = UDP_CLIENT.Receive(ref ENDPOINT);
                        VideoReceived?.Invoke(dgram_.Concat(dgram__).ToArray());
                    }
                }
            }

            catch
            {
                
            }

            finally
            {
                BeginReceive();
            }
        }
    }
}