using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCWS_Situation_room
{
    internal class Packet
    {
        public struct SEND_PACKET
        {
            public int BodyPan;
            public int BodyTilt;
            public uint Button;
            public short C_X;
            public short C_Y;
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