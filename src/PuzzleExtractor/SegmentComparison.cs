using System;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace PuzzleExtractor
{
    public class SegmentComparison
    {
        public SegmentComparison(Segment a, Segment b)
        {
            A = a;
            B = b;

            Image = GetComparisonImage(A.Comparable, B.Comparable);
            Distance = Image.CountNonzero().First();
        }


        public Segment A { get; set; }
        public Segment B { get; set; }

        public Image<Bgr, byte> Image { get; }
        public int Distance { get; set; }

        private Image<Bgr, byte> GetComparisonImage(PointF[] a, PointF[] b)
        {
            var joinedPoly = a.Concat(b.Reverse()).ToArray();

            var image = new Image<Bgr, byte>(new Size(400, 400));

            var contour = new VectorOfPoint(Offset(Round(joinedPoly), 100).ToArray());

            CvInvoke.FillPoly(image, new VectorOfVectorOfPoint(contour), new Bgr(Color.Blue).MCvScalar);

            return image;
        }

        public static Point[] Round(PointF[] segment)
        {
            return segment.Select(p => new Point((int)Math.Round(p.X), (int)Math.Round(p.Y))).ToArray();
        }

        public static Point[] Offset(Point[] segment, int offset)
        {
            return segment.Select(p => new Point(p.X + offset, p.Y + 100)).ToArray();
        }
    }
}