using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCWS_Situation_room
{
    public class Frame_Processing
    {
        public Bitmap DecodeFrame(byte[] frameData)
        {
            using (var ms = new MemoryStream(frameData))
            {
                return new Bitmap(ms);
            }
        }
    }
}
