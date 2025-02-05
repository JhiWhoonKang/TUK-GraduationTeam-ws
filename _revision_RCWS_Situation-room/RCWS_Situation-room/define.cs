using System.Net.Sockets;

namespace RCWS_Situation_room.define
{
    // Login
    partial class Define
    {
        public const string ID = "까불면 메카 약이다";
        public const string PW = "mecha";
    }

    // 소켓
    public partial class Define
    {
        public const string SERVER_IP = "192.168.0.30";
        //public const string SERVER_IP = "127.0.0.1";

        public UdpClient udpClient;

        public const int TCPPORT = 7000;
        public const int UDPPORT = 8000;
        public const int UDPPORT2 = 9002; //              
        public const int TEST_UDPPORT = 12345;
    }

    // 조이스틱 관련
    public partial class Define
    {
        public const uint BUTTON_1_MASK = 0x01;
        public const uint BUTTON_2_MASK = 0x02;
        public const uint BUTTON_3_MASK = 0x04;
        public const uint BUTTON_4_MASK = 0x08;
        public const uint BUTTON_5_MASK = 0x10;
        public const uint BUTTON_6_MASK = 0x20;
        public const uint BUTTON_7_MASK = 0x20000;
        public const uint BUTTON_8_MASK = 0x80;
        public const uint BUTTON_9_MASK = 0x40000;
        public const uint BUTTON_10_MASK = 0x200;
        public const uint BUTTON_11_MASK = 0x400;
        public const uint BUTTON_12_MASK = 0x800;
        public const uint MOUSE_BUTTON_LEFT_MASK = 0x00001000;
        public const uint MOUSE_BUTTON_RIGHT_MASK = 0x00002000;
        public const uint MOUSE_BUTTON_RESET_MASK = 0x00003000;
        public const uint DISCONNECT_BUTTON_MASK = 0x04;
        public const uint POWER_ON_BUTTON_MASK = 0x100000;
    }

    // 영상 관련
    public partial class Define
    {
        public const ushort VIDEO_WIDTH = 1152;
        public const ushort VIDEO_HEIGHT = 864;
        public const double VIDEO_SCALE = 1.8;
    }

    // 제한 각도
    public partial class Define
    {
        public const int MAX_ANGLE = 0;
        public const int MIN_ANGLE = 0;
    }

    // 지도 거리 스케일
    public partial class Define
    {
        public static readonly int LENGTH_SCALE = 18;
        public static float CURRENT_SCALE = 1.0f;
        public static readonly float ZOOM_FACTOR = 1.1f;
    }

    // 광학 관련
    public partial class Define
    {
        public const int MIN_MAGNIFICATION = 1;
        public const int MAX_MAGNIFICATION = 5;
    }
}