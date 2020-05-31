using System.Drawing;
using System.IO;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;

namespace PuzzleExtractor
{
    public class Batch
    {
        private readonly string _sourceFilePath;
        private readonly Analysis.Options _options;

        private Image<Gray, byte> _contourImage;

        public Batch(string sourceFilePath, Analysis.Options options)
        {
            _sourceFilePath = sourceFilePath;
            _options = options;

            FileName = Path.GetFileNameWithoutExtension(_sourceFilePath);
        }
        public string FileName { get; }

        public Piece[] Pieces { get; private set; }

        public Bitmap SourceImage { get; private set; }
        public Bitmap CornerImage { get; private set; }
        public Bitmap SegmentImage { get; private set; }

        public Bitmap ContourImage => _contourImage.Bitmap;

        public void Process()
        {
            SourceImage = new Bitmap(_sourceFilePath);

            var grey = new Image<Gray, byte>(SourceImage);
            //var smooth = grey.SmoothGaussian(7);
            _contourImage = grey.ThresholdBinary(new Gray(_options.ContourThreshold), new Gray(256));
            var contours = ContourExtractor.GetContours(_contourImage);

            CornerImage = CornerExtractor.GetCornerImage(_contourImage, _options.CornerThreshold).Bitmap;
            var corners = CornerExtractor.GetRealCorners(_contourImage, _options.CornerThreshold);

            Pieces = contours
                .Select(c =>
                {
                    var contourCorners = CornerExtractor.GetContourCorners(c, corners);
                    return contourCorners == null ? null : new Piece(c, CornerExtractor.GetContourCorners(c, corners), this);
                })
                .Where(p => p != null)
                .ToArray();

            SegmentImage = GetSegmentImage();
        }

        public Bitmap GetSegmentImage()
        {
            var displayImage = new Image<Bgr, byte>(CornerImage);

            var colors = new[] { Color.Yellow, Color.Red, Color.Orange, Color.Cyan, Color.Indigo, Color.Chartreuse };

            foreach (var piece in Pieces)
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

            return displayImage.Bitmap;
        }
    }
}