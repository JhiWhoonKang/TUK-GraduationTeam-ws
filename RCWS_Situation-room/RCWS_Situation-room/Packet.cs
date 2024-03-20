using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCWS_Situation_room
{
    internal class Packet
    {
        public struct SendTCP
        {
            public int BodyTilt;
            public int BodyPan;
            public byte Permission;
            public byte Fire;
            public byte TakeAim;
            public byte Magnification;
        }

        public struct ReceiveTCP
        {
            public float OpticalTilt;
            public float OpticalPan;
            public float BodyTilt;
            public float BodyPan;
            public float GunVoltage;
            public int distance;
            public int pointdistance;
            public byte Permission;
            public byte TakeAim;
            public byte Remaining_bullets;
            public byte Magnification;
            public byte Fire;
        }
    }
}