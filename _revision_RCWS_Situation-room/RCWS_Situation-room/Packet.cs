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
            public float OpticalTilt;
            public float OpticalPan;
            public float BodyTilt;
            public float BodyPan;
        }

        public struct SEND_PACKET_UDP
        {
            public int Left_or_Right;
            public int X;
            public int Y;
        }
    }
}