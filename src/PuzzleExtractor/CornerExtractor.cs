using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace PuzzleExtractor
{
    public class CornerExtractor
    {
        public static Image<Gray, float> GetCornerImage(Image<Gray, byte> contourImage, double cornerThreshold)
        {
            var cornerImage = GetRawCornerImage(contourImage);

            var cornersThresholded = cornerImage.ThresholdBinaryInv(new Gray(cornerThreshold), new Gray(256));
            return cornersThresholded.Erode(1);
        }

        private static Image<Gray, float> GetRawCornerImage(Image<Gray, byte> contourImage)
        {
            var cornerImage = new Image<Gray, float>(contourImage.Size);
            CvInvoke.CornerHarris(contourImage, cornerImage, 7);
            return cornerImage;
        }

        public static List<Point> GetRealCorners(Image<Gray, byte> contourImage, double cornerThreshold)
        {
            var rawCornerImage = GetRawCornerImage(contourImage);

            var cornersThresholded = rawCornerImage.ThresholdBinaryInv(new Gray(cornerThreshold), new Gray(256));
            cornersThresholded = cornersThresholded.Erode(1);

            var approxCorners = GetCorners(new Image<Gray, byte>(cornersThresholded.Bitmap));

            var cornerData = rawCornerImage.Data;

            return approxCorners.Select(c => FindLocalMax(cornerData, c)).ToList();
        }

        private static Point FindLocalMax(float[,,] rawCornerImage, Point point)
        {
            var deltas = new[]
            {
                Tuple.Create(1, 0),
                Tuple.Create(0, 1),
                Tuple.Create(-1, 0),
                Tuple.Create(0, -1),
                Tuple.Create(1, 1),
                Tuple.Create(-1, 1),
                Tuple.Create(-1, -1),
                Tuple.Create(1, -1),
            };

            foreach (var delta in deltas)
            {
                var p = new Point(point.X + delta.Item1, point.Y + delta.Item2);
                if (p.X >= rawCornerImage.GetUpperBound(0)) continue;
                if (p.Y >= rawCornerImage.GetUpperBound(1)) continue;
                if (p.X < 0 || p.Y < 0) continue;
                if (rawCornerImage[p.X, p.Y, 0] > rawCornerImage[point.X, point.Y, 0])
                {
                    return FindLocalMax(rawCornerImage, p);
                }
            }

            return point;
        }

        public static List<Point> GetCorners(Image<Gray, byte> cornerImage)
        {
            var result = new List<Point>();

            using (var vectorOfContours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(cornerImage, vectorOfContours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);

                for (var i = 0; i < vectorOfContours.Size; i++)
                {
                    var contour = vectorOfContours[i];
                    //if (CvInvoke.ContourArea(contour) > 50) continue;

                    var centroid = Utils.GetCentroid(contour.ToArray());

                    result.Add(centroid);
                }

                return result;
            }
        }

        //public static Point GetCentroid(Point[] points)
        //{
        //    return GetCentroid(new VectorOfPoint(points));
        //}

        //public static Point GetCentroid(VectorOfPoint points)
        //{
        //    var moments = CvInvoke.Moments(points, true);
        //    var centroid = new Point((int) (moments.M10 / moments.M00), (int) (moments.M01 / moments.M00));
        //    return centroid;
        //}

        public static T[][] GetCombinations<T>(T[] list, int length)
        {
            var res = new List<T[]>();

            for (var i = 0; i <= list.Length - length; i++)
            {
                var curr = new[] {list[i]};

                if (length == 1)
                {
                    res.Add(curr);
                }
                else
                {
                    var r = GetCombinations(list.Skip(i + 1).ToArray(), length - 1)
                        .Select(c => new[] {list[i]}.Concat(c).ToArray()).ToArray();
                    res.AddRange(r);
                }
            }
                
            return res.ToArray();
        }

        public static Point[] GetContourCorners(Point[] contour, List<Point> corners)
        {
            var rectangle = CvInvoke.BoundingRectangle(new VectorOfPoint(contour));
            var cornersInRectangle = corners.Where(c => rectangle.Contains(c)).ToArray();

            var adjustedCorners = cornersInRectangle.ToArray();
            var distances = cornersInRectangle.Select(c => int.MaxValue).ToArray();

            foreach (var contourPoint in contour)
            {
                for (var i = 0; i < cornersInRectangle.Length; i++)
                {
                    var distance = Utils.DistanceSquared(cornersInRectangle[i], contourPoint);
                    if (distance < distances[i])
                    {
                        adjustedCorners[i] = contourPoint;
                        distances[i] = distance;
                    }
                }
            }

            var finalCorners = adjustedCorners.Where((c, i) => distances[i] < 10).ToArray();

            if (finalCorners.Length > 4)
            {
                finalCorners = ChoseCornersByCentroid(contour, finalCorners);
            }

            return finalCorners;
        }

        private static Point[] ChoseCornersByCentroid(Point[] contour, Point[] finalCorners)
        {
            var contourCentroid = Utils.GetCentroid(contour);

            var combinations = GetCombinations(finalCorners, 4);

            var yo = combinations
                .Select(c => Tuple.Create(c, Utils.DistanceSquared(contourCentroid, Utils.GetCentroid(c))))
                .OrderBy(t => t.Item2)
                .ToArray();

            return yo.First().Item1;
        }
    }
}
