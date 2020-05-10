using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace PuzzleExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            var img = new Image<Bgr, Byte>(DataDir.Contours("multipieces.jpg"));
            var grey = new Image<Gray, byte>(img.Bitmap);
            var smooth = grey.SmoothGaussian(7);
            var thresholded = smooth.ThresholdBinary(new Gray(160), new Gray(256));

            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                // Build list of contours
                CvInvoke.FindContours(thresholded, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);

                var contourAreas = Enumerable.Range(0, contours.Size).Select(i => CvInvoke.ContourArea(contours[i]))
                    .ToArray();

                var filteredAreas = contourAreas.Where(a => a > 500).OrderBy(a => a).ToArray();
                var median = filteredAreas.Skip(filteredAreas.Length / 2).FirstOrDefault();

                const double deviation = 0.2;
                var from = median * (1 - deviation);
                var to = median * (1 + deviation);

                for (int i = 0; i < contours.Size; i++)
                {
                    var area = contourAreas[i];
                    if (area < from || area > to) continue;

                    var contour = contours[i];

                    CvInvoke.Polylines(img, contour, true, new Bgr(Color.Red).MCvScalar, 4);
                }
            }

            var cornerImage = new Image<Gray, float>(thresholded.Size);
            CvInvoke.CornerHarris(thresholded, cornerImage, 3);
            var cornersThresholded = cornerImage.ThresholdBinaryInv(new Gray(160), new Gray(256));

            cornersThresholded.Bitmap.Save(DataDir.Contours("cornerImage.bmp"), ImageFormat.Bmp);

            img.Bitmap.Save(DataDir.Contours("markedContours.bmp"), ImageFormat.Bmp);
        }


    }
}
