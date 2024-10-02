using System.Net.Sockets;

namespace RCWS_Situation_room
{
    internal class Define
    {
        //public const string SERVER_IP = "192.168.0.30";
        public const string SERVER_IP = "127.0.0.1";

        public UdpClient udpClient;

        public const int TCPPORT = 7000;
        public const int UDPPORT = 8000;
        public const int UDPPORT2 = 9002; // 

        public const ushort VIDEO_WIDTH = 1152;
        public const ushort VIDEO_HEIGHT = 864;

        public const int MAX_ANGLE = 0;
        public const int MIN_ANGLE = 0;

        public const int MIN_MAGNIFICATION = 1;
        public const int MAX_MAGNIFICATION = 5;

        public const double VIDEO_SCALE = 1.8;

        public const int LENGTH_SCALE = 18;
    }
}