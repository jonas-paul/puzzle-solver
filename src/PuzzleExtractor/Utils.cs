using System;
using System.Drawing;
using System.Linq;

namespace PuzzleExtractor
{
    public class Utils
    {
        public static Point GetCentroid(Point[] c)
        {
            return new Point((int)Math.Round(c.Average(p => p.X)), (int)Math.Round(c.Average(p => p.Y)));
        }

        public static int DistanceSquared(Point a, Point b)
        {
            return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
        }
    }
}
