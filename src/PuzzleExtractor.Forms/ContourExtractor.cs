using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace PuzzleExtractor.Forms
{
    public static class ContourExtractor
    {
        public static List<Point[]> GetContours(Image<Gray, byte> contourImage)
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
                if (area < @from || area > to) continue;

                result.Add(vectorOfContours[i].ToArray());
            }

            return result;
        }
    }
}