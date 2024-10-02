using System.Collections.Generic;
using System.Drawing;

namespace RCWS_Situation_room
{
    public struct PinPoint
    {
        public Point Center;
        public float Radius;

        public PinPoint(Point center, float radius)
        {
            Center = center;
            Radius = radius;
        }
    }

    internal class Draw
    {
        private List<PinPoint> pinpoint = new List<PinPoint>();

        public void AddPinPoint(Point center, float radius)
        {
            pinpoint.Add(new PinPoint(center, radius));
        }

        public void DeletePinPoint(Point point)
        {
            pinpoint.RemoveAll(pinpoint => IsPointInCircle(point, pinpoint));
        }

        public void DeleteAllPinPoints()
        {
            pinpoint.Clear();
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
                g.DrawEllipse(Pens.Red, circle.Center.X - circle.Radius, circle.Center.Y - circle.Radius, circle.Radius * 2, circle.Radius * 2);
            }
        }
    }
}