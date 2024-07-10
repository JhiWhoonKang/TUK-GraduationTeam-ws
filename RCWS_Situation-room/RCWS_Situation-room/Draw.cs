using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Web.UI.WebControls;

namespace RCWS_Situation_room
{
    public struct PinPoint
    {
        public Point Center;
        public int Radius;

        public PinPoint(Point center, int radius)
        {
            Center = center;
            Radius = radius;
        }
    }

    internal class Draw
    {
        private List<PinPoint> pinpoint = new List<PinPoint>();

        public void AddPinPoint(Point point)
        {
            pinpoint.Add(new PinPoint(point, 10));
        }

        public void DeletePinPoint(Point point)
        {
            pinpoint.RemoveAll(pinpoint => IsPointInCircle(point, pinpoint));
        }

        private bool IsPointInCircle(Point point, PinPoint circle)
        {
            return (circle.Center.X - point.X) * (circle.Center.X - point.X) +
                   (circle.Center.Y - point.Y) * (circle.Center.Y - point.Y) <= circle.Radius * circle.Radius;
        }

        public void Drawing(Graphics g)
        {
            foreach (var circle in pinpoint)
            {
                g.DrawEllipse(Pens.Black, circle.Center.X - circle.Radius, circle.Center.Y - circle.Radius, circle.Radius * 2, circle.Radius * 2);
            }
        }
    }
}
