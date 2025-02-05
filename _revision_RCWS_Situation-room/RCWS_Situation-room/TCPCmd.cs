using System;
using System.IO;
using System.Runtime.InteropServices;

namespace RCWS_Situation_room.Data
{
    public class TCPCmd
    {
        //public static byte[] StructToBytes<T>(T data) where T : struct
        //{
        //    using (var ms = new MemoryStream())
        //    {
        //        using (var writer = new BinaryWriter(ms))
        //        {
        //            if (data is Packet.SEND_PACKET sendPacket)
        //            {
        //                writer.Write(sendPacket.BODY_PAN);
        //                writer.Write(sendPacket.OPTICAL_TILT);
        //                writer.Write(sendPacket.Button);
        //                writer.Write(sendPacket.C_X1);
        //                writer.Write(sendPacket.C_Y1);
        //                writer.Write(sendPacket.D_X1);
        //                writer.Write(sendPacket.D_Y1);
        //                writer.Write(sendPacket.D_X2);
        //                writer.Write(sendPacket.D_Y2);
        //            }

        //            writer.Flush();
        //            return ms.ToArray();
        //        }
        //    }                
        //}

        //public static T BytesToStruct<T>(byte[] bytes) where T : struct
        //{
        //    using (var ms = new MemoryStream(bytes))
        //    {
        //        using (var reader = new BinaryReader(ms))
        //        {
        //            if (typeof(T) == typeof(Packet.RECEIVED_PACKET))
        //            {
        //                var packet = new Packet.RECEIVED_PACKET
        //                {
        //                    OPTICAL_TILT = reader.ReadSingle(),
        //                    WEAPON_TILT = reader.ReadSingle(),
        //                    BODY_PAN = reader.ReadSingle(),
        //                    SENTRY_AZIMUTH = reader.ReadSingle(),
        //                    SENTRY_ELEVATION = reader.ReadSingle(),
        //                    DISTANCE = reader.ReadSingle(),
        //                    PERMISSION = reader.ReadByte(),
        //                    TAKE_AIM = reader.ReadByte(),
        //                    FIRE = reader.ReadByte(),
        //                    MODE = reader.ReadByte()
        //                };
        //                return (T)(object)packet;
        //            }
        //            throw new InvalidOperationException("Unsupported type");
        //        }
        //    }            
        //}

        public static byte[] StructToBytes<T>(T structure) where T : struct
        {
            int size = Marshal.SizeOf(structure);
            byte[] bytes = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(structure, ptr, true);
            Marshal.Copy(ptr, bytes, 0, size);
            Marshal.FreeHGlobal(ptr);

            return bytes;
        }

        public static T BytesToStruct<T>(byte[] bytes) where T : struct
        {
            int size = Marshal.SizeOf<T>();
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(bytes, 0, ptr, size);
            T structure = Marshal.PtrToStructure<T>(ptr);
            Marshal.FreeHGlobal(ptr);

            return structure;
        }
    }
}