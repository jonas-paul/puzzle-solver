using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PuzzleExtractor
{
    public class Piece
    {
        public Piece(Point[] contour, Point[] corners, Batch batch)
        {
            Contour = contour;
            Corners = corners;
            Batch = batch;
            Segments = SplitToSegments(contour, corners);
        }

        public Point[] Contour { get; }
        public Point[] Corners { get; }
        public Batch Batch { get; }
        public Segment[] Segments { get; set; }

        public Segment[] SplitToSegments(Point[] contour, Point[] corners)
        {
            var contourPoints = contour.ToArray().Reverse().ToArray();

            var pointsToShift = contourPoints.TakeWhile(p => !corners.Contains(p)).Count() + 1;
            var contourPointsShifted = contourPoints.Skip(pointsToShift)
                .Concat(contourPoints.Take(pointsToShift)).ToArray();

            var segments = new List<ArraySegment<Point>>();
            var start = 0;

            for (var i = 0; i < contourPointsShifted.Length; i++)
            {
                if (corners.Contains(contourPointsShifted[i]))
                {
                    segments.Add(new ArraySegment<Point>(contourPointsShifted, start, i - start));
                    start = i + 1;
                }
            }

            return segments.Where(s => s.Any()).Select(s => new Segment(s, this)).ToArray();
        }
    }
}