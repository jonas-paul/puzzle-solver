using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Serilog;

namespace PuzzleExtractor
{
    public class Analysis
    {
        private readonly string[] _sourceImagePaths;

        public Analysis(string[] sourceImagePaths)
        {
            _sourceImagePaths = sourceImagePaths;
        }

        public List<Batch> Batches { get; set; }

        public List<SegmentComparison> Comparisons { get; private set; }

        public void Recalculate(Options options)
        {
            Batches = _sourceImagePaths.Select(p => new Batch(p, options)).ToList();
            foreach (var batch in Batches)
            {
                batch.Process();
            }
        }

        public void CompareSegments(CancellationToken token)
        {
            var allSegments = Batches.SelectMany(b => b.Pieces.SelectMany(p => p.Segments)).ToArray();

            var groups = allSegments.GroupBy(s => s.IsPointy).ToArray();

            Comparisons = new List<SegmentComparison>();

            var hardTofillSegments = allSegments.ToDictionary(s => s, s => 0);

            if (groups.Length != 2)
            {
                Log.Error($"Expected 2 groups but was {groups.Length}");
                return;
            }

            foreach (var a in groups.First())
            {
                foreach (var b in groups.Last())
                {
                    if (a.Piece != b.Piece
                        && hardTofillSegments[a] <= 1
                        && hardTofillSegments[b] <= 1)
                    {
                        if (token.IsCancellationRequested) return;

                        var sw = new Stopwatch();
                        sw.Start();

                        Comparisons.Add(new SegmentComparison(a, b));

                        if (sw.Elapsed > TimeSpan.FromSeconds(3))
                        {
                            hardTofillSegments[a]++;
                            hardTofillSegments[b]++;
                        }
                    }
                }
            }

            Comparisons = Comparisons.OrderBy(c => c.Distance).ToList();

            //var displayImage = Batches.First().GetDisplayImage();

            //var colors = new[] { Color.Yellow, Color.Red, Color.Orange, Color.Cyan, Color.Indigo, Color.Chartreuse };

            //for (var i = 0; i < Math.Min(colors.Length, Comparisons.Count); i++)
            //{
            //    var comp = Comparisons[i];
            //    CvInvoke.PutText(displayImage, i.ToString(), comp.A.Centroid, FontFace.HersheySimplex, 1.0,
            //        new Bgr(colors[i]).MCvScalar, 2);
            //    CvInvoke.PutText(displayImage, i.ToString(), comp.B.Centroid, FontFace.HersheySimplex, 1.0,
            //        new Bgr(colors[i]).MCvScalar, 2);
            //}
        }

        public class Options
        {
            public int ContourThreshold { get; set; }
            public double CornerThreshold { get; set; }
        }
    }
}