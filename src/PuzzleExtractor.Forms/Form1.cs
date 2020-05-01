using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace PuzzleExtractor.Forms
{
    public partial class Form1 : Form
    {
        private readonly Bitmap _sourceImage;
        private List<SegmentComparison> _comparisons;
        private int _comparisonsIndex = 0;

        public Form1()
        {
            InitializeComponent();

            _sourceImage = new Bitmap(@"C:\Users\Jonas\Desktop\contours\test4.jpg");

            trackBar1.Value = 12;

            Recalculate(trackBar1.Value);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            lock (_sourceImage)
            {
                var trackBar = (TrackBar)sender;
                Recalculate(trackBar.Value);
            }
        }

        private void Recalculate(int trackBarValue)
        {
            const double scaleFactor = 1000;
            var threshold = trackBarValue / scaleFactor;

            label1.Text = $"threshold: {threshold.ToString(CultureInfo.InvariantCulture)}";

            var grey = new Image<Gray, byte>(_sourceImage);
            //var smooth = grey.SmoothGaussian(7);
            var contourImage = grey.ThresholdBinary(new Gray(175), new Gray(256));

            var cornerImage = CornerExtractor.GetCornerImage(contourImage, threshold).Bitmap;
            var corners = CornerExtractor.GetRealCorners(contourImage, threshold);

            var displayImage = new Image<Bgr, byte>(contourImage.Bitmap);
            //var displayImage = new Image<Bgr, byte>(cornerImage);

            var allSegments = new List<Segment>();

            var colors = new[] { Color.Yellow, Color.Red, Color.Orange, Color.Cyan, Color.Indigo, Color.Chartreuse };

            var contours = GetContours(contourImage);

            foreach (var contour in contours)
            {
                var rectangle = CvInvoke.BoundingRectangle(new VectorOfPoint(contour));
                var cornersInRectangle = corners.Where(c => rectangle.Contains(c)).ToArray();

                var adjustedCorners = cornersInRectangle.ToArray();
                var distances = cornersInRectangle.Select(c => int.MaxValue).ToArray();

                var contourPoints = contour.ToArray().Reverse().ToArray();

                foreach (var contourPoint in contourPoints)
                {
                    for (var i = 0; i < cornersInRectangle.Length; i++)
                    {
                        var distance = DistanceSquared(cornersInRectangle[i], contourPoint);
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
                    var contourCentroid = CornerExtractor.GetCentroid(contour);

                    var comb = CornerExtractor.GetCombinations(finalCorners, 4);
                    
                    var yo = comb.Select(c => Tuple.Create(c,
                            DistanceSquared(contourCentroid,
                                new Point((int)Math.Round(c.Sum(p => p.X) / 4m), (int)Math.Round(c.Sum(p => p.Y) / 4m)))))
                        .ToArray();

                    var minDistance = yo.Min(d => d.Item2);
                    var chosenCorners = yo.First(d => d.Item2 == minDistance).Item1;
                    finalCorners = chosenCorners;
                }

                CvInvoke.Polylines(displayImage, contour, true, new Bgr(Color.Red).MCvScalar, 1);

                var pointsToShift = contourPoints.TakeWhile(p => !finalCorners.Contains(p)).Count() + 1;
                var contourPointsShifted = contourPoints.Skip(pointsToShift)
                    .Concat(contourPoints.Take(pointsToShift)).ToArray();

                var segments = new List<ArraySegment<Point>>();
                var start = 0;

                for (var i = 0; i < contourPointsShifted.Length; i++)
                {
                    if (finalCorners.Contains(contourPointsShifted[i]))
                    {
                        segments.Add(new ArraySegment<Point>(contourPointsShifted, start, i - start));
                        start = i + 1;
                    }
                }

                allSegments.AddRange(segments.Where(s => s.Any()).Select(s => new Segment(s, contour.GetHashCode())));


                for (var i = 0; i < segments.Count; i++)
                {
                    displayImage.DrawPolyline(segments[i].ToArray(), false, new Bgr(colors[i%colors.Length]));
                }

                foreach (var corner in finalCorners)
                {
                    displayImage.Draw(new Rectangle(corner, new Size(3, 3)), new Bgr(Color.Red));
                }
            }

            //imageBox1.Image = displayImage.Bitmap;

            var groups = allSegments.GroupBy(s => s.IsPointy).ToArray();

            _comparisons = new List<SegmentComparison>();

            var hardToFillContours = allSegments.Select(s => s.ContourHashCode).Distinct().ToDictionary(c => c, c => 0);

            if (groups.Length == 2)
            {
                foreach (var a in groups.First())
                {
                    foreach (var b in groups.Last())
                    {
                        if (a.ContourHashCode != b.ContourHashCode
                            && hardToFillContours[a.ContourHashCode] <= 1
                            && hardToFillContours[b.ContourHashCode] <= 1)
                        {
                            var sw = new Stopwatch();
                            sw.Start();

                            _comparisons.Add(new SegmentComparison(a, b));

                            if (sw.Elapsed > TimeSpan.FromSeconds(3))
                            {
                                hardToFillContours[a.ContourHashCode]++;
                                hardToFillContours[b.ContourHashCode]++;
                            }
                        }
                    }
                }

                _comparisons = _comparisons.OrderBy(c => c.Distance).ToList();
            }

            var resultView = new Image<Bgr,byte>(_sourceImage);

            for (var i = 0; i < Math.Min(colors.Length, _comparisons.Count); i++)
            {
                var crossSize = 30;

                var comp = _comparisons[i];
                displayImage.Draw(new Cross2DF(comp.A.Centroid, crossSize, crossSize), new Bgr(colors[i]), 5);
                displayImage.Draw(new Cross2DF(comp.B.Centroid, crossSize, crossSize), new Bgr(colors[i]), 5);
            }

            imageBox1.Image = displayImage.Bitmap;
        }

        public List<Point[]> GetContours(Image<Gray, byte> contourImage)
        {
            var result = new List<Point[]>();

            var vectorOfContours = new VectorOfVectorOfPoint();

            CvInvoke.FindContours(contourImage, vectorOfContours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);

            var contourAreas = Enumerable.Range(0, vectorOfContours.Size).Select(i => CvInvoke.ContourArea(vectorOfContours[i]))
                .ToArray();

            var filteredAreas = contourAreas.Where(a => a > 500).OrderBy(a => a).ToArray();
            var median = filteredAreas.Skip(filteredAreas.Length / 2).FirstOrDefault();

            const double deviation = 0.2;
            var from = median * (1 - deviation);
            var to = median * (1 + deviation);

            for (var i = 0; i < vectorOfContours.Size; i++)
            {
                var area = contourAreas[i];
                if (area < from || area > to) continue;

                result.Add(vectorOfContours[i].ToArray());
            }

            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            imageBox1.Image.Save(Desktop("SavedFromForms.bmp"), ImageFormat.Bmp);
        }

        static int DistanceSquared(Point a, Point b)
        {
            return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
        }

        static string Desktop(string filename)
        {
            return Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "contours"), filename);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var comparison = _comparisons[_comparisonsIndex];
            
            areaLabel.Text = comparison.Distance.ToString();

            imageBox1.Image = comparison.Image.Bitmap;

            _comparisonsIndex++;
        }
    }
}
