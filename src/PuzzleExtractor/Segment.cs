using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace PuzzleExtractor
{
    public class Segment
    {
        public Segment(IEnumerable<Point> points, Piece piece)
        {
            Piece = piece;
            Points = points.ToArray();

            Aligned = Align(Points);
            Comparable = IsPointy ? Aligned : FlipAndReverse(Aligned);
        }

        public Point[] Points { get; }

        public Piece Piece { get; }

        public bool IsPointy => Aligned.Sum(p => p.Y * Math.Abs(p.Y)) > 0;

        public PointF[] Aligned { get; }

        public PointF[] Comparable { get; }

        public Point Centroid => Utils.GetCentroid(Points);

        public static PointF[] Align(Point[] segment)
        {
            var start = segment[0];
            var result = segment.Select(p => new PointF(p.X - start.X, p.Y - start.Y)).ToArray();

            var end = result.Last();

            var matrix = new Matrix();
            var add = end.X < 0 ? 180 : 0;
            var angle = (float) (-Math.Atan(end.Y / end.X) * 180 / Math.PI + add);
            matrix.Rotate(angle);

            matrix.TransformPoints(result);

            return result;
        }

        public static PointF[] FlipAndReverse(PointF[] segment)
        {
            var last = segment.Last();

            return segment.Select(p => new PointF(last.X - p.X, -p.Y)).Reverse().ToArray();
        }
    }
}
