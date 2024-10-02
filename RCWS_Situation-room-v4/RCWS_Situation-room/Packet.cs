namespace RCWS_Situation_room
{
    public class Packet
    {
        public struct SEND_PACKET
        {
            public int BODY_PAN;
            public int OPTICAL_TILT;
            public uint Button;
            public short C_X1;
            public short C_Y1;
            public short D_X1;
            public short D_Y1;
            public short D_X2;
            public short D_Y2;
        }

        public struct RECEIVED_PACKET
        {
            public float OPTICAL_TILT;
            public float WEAPON_TILT; //기총
            public float BODY_PAN; //바디
            public float SENTRY_AZIMUTH;
            public float SENTRY_ELEVATION;
            public float DISTANCE;
            public byte PERMISSION;
            public byte TAKE_AIM;
            public byte FIRE;
            public byte MODE;
        }
    }
}