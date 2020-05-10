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

            _sourceImage = new Bitmap(DataDir.Contours("test4.jpg"));

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

        public class PuzzleImage
        {
            private readonly string _sourceFilePath;
            private readonly Options _options;
            private string _fileName;
            private Bitmap _sourceImage;
            private Bitmap _cornerImage;

            public PuzzleImage(string sourceFilePath, Options options)
            {
                _sourceFilePath = sourceFilePath;
                _options = options;

                _fileName = Path.GetFileNameWithoutExtension(_sourceFilePath);
            }

            public void Process()
            {
                _sourceImage = new Bitmap(_sourceFilePath);

                var grey = new Image<Gray, byte>(_sourceImage);
                //var smooth = grey.SmoothGaussian(7);
                var contourImage = grey.ThresholdBinary(new Gray(175), new Gray(256));

                _cornerImage = CornerExtractor.GetCornerImage(contourImage, _options.CornerThreshold).Bitmap;
                var corners = CornerExtractor.GetRealCorners(contourImage, _options.CornerThreshold);

                var allSegments = new List<Segment>();

            }

            public class Options
            {
                public int ContourThreshold { get; set; }
                public int CornerThreshold { get; set; }
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

            var colors = new[] { Color.Yellow, Color.Red, Color.Orange, Color.Cyan, Color.Indigo, Color.Chartreuse };

            var contours = ContourExtractor.GetContours(contourImage);

            var pieces = contours.Select(c => new Piece(c, CornerExtractor.GetContourCorners(c, corners))).ToArray();

            foreach (var piece in pieces)
            {
                CvInvoke.Polylines(displayImage, piece.Contour, true, new Bgr(Color.Red).MCvScalar, 1);

                for (var i = 0; i < piece.Segments.Count(); i++)
                {
                    displayImage.DrawPolyline(piece.Segments[i].Points, false, new Bgr(colors[i % colors.Length]));
                }

                foreach (var corner in piece.Corners)
                {
                    displayImage.Draw(new Rectangle(corner, new Size(3, 3)), new Bgr(Color.Red));
                }
            }

            //imageBox1.Image = displayImage.Bitmap;

            var allSegments = pieces.SelectMany(p => p.Segments).ToArray();

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
                CvInvoke.PutText(displayImage, i.ToString(), comp.A.Centroid, FontFace.HersheySimplex, 1.0, new Bgr(colors[i]).MCvScalar, 2);
                CvInvoke.PutText(displayImage, i.ToString(), comp.B.Centroid, FontFace.HersheySimplex, 1.0, new Bgr(colors[i]).MCvScalar, 2);
                //displayImage.Draw(new Cross2DF(comp.A.Centroid, crossSize, crossSize), new Bgr(colors[i]), 5);
                //displayImage.Draw(new Cross2DF(comp.B.Centroid, crossSize, crossSize), new Bgr(colors[i]), 5);
            }

            imageBox1.Image = displayImage.Bitmap;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            imageBox1.Image.Save(DataDir.Contours("SavedFromForms.bmp"), ImageFormat.Bmp);
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
