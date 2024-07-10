using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RCWS_Situation_room
{
    internal class define
    {
        //public const string SERVER_IP = "10.254.1.20";
        public const string SERVER_IP = "127.0.0.1";
        //public const string SERVER_IP = "172.16.1.227";
        //public const string SERVER_IP = "172.16.2.249";
        //public const string SERVER_IP = "192.168.0.53";

        public UdpClient udpClient;

        public const int TCPPORT = 7000;
        public const int UDPPORT = 9000; // python
        public const int UDPPORT2 = 9002; // 

        public const int MAX_ANGLE = 0;
        public const int MIN_ANGLE = 0;
    }
}