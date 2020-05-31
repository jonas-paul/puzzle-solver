using System;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleExtractor.Forms
{
    public partial class Form1 : Form
    {
        private readonly Analysis _analysis;
        private readonly View _view;
        private readonly Analysis.Options _options;
        
        public Form1()
        {
            InitializeComponent();

            _analysis = new Analysis(DataDir.AllPiecesFiles().Take(1).ToArray());

            _view = new View
            {
                Mode = ViewMode.Batch,
                BatchMode = BatchViewMode.Corner,
                BatchIndex = 0,
                ComparisonIndex = 0
            };

            _options = new Analysis.Options
            {
                ContourThreshold = 175,
            };

            cornerThresholdTextBox.Text = "0.012";

            Recalculate();
        }

        private void Recalculate()
        {
            _analysis.Recalculate(_options);

            Task.Run(() => _analysis.CompareSegments(CancellationToken.None));

            UpdateForm();
        }

        private void UpdateViewModes()
        {
            if (batchMode.Checked) _view.Mode = ViewMode.Batch;
            if (comparisonMode.Checked) _view.Mode = ViewMode.Comparison;

            if (sourceBatchViewMode.Checked) _view.BatchMode = BatchViewMode.Source;
            if (contourBatchViewMode.Checked) _view.BatchMode = BatchViewMode.Contour;
            if (cornerBatchViewMode.Checked) _view.BatchMode = BatchViewMode.Corner;
            if (segmentBatchViewMode.Checked) _view.BatchMode = BatchViewMode.Segment;

            UpdateForm();
        }

        private void UpdateViewIndex(bool next)
        {
            var count = _view.Mode == ViewMode.Batch ? _analysis.Batches.Count : _analysis.Comparisons.Count;
            var current = _view.Mode == ViewMode.Batch ? _view.BatchIndex : _view.ComparisonIndex;

            var nextIdx = CalcIndex(count, current, next);

            if (_view.Mode == ViewMode.Batch)
                _view.BatchIndex = nextIdx;
            else
                _view.ComparisonIndex = nextIdx;

            UpdateForm();
        }

        private void UpdateForm()
        {
            if (_view.Mode == ViewMode.Batch)
            {
                var batch = _analysis.Batches[_view.BatchIndex];

                areaLabel.Text = batch.FileName;

                switch (_view.BatchMode)
                {
                    case BatchViewMode.Source:
                        imageBox1.Image = batch.SourceImage;
                        break;
                    case BatchViewMode.Contour:
                        imageBox1.Image = batch.ContourImage;
                        break;
                    case BatchViewMode.Corner:
                        imageBox1.Image = batch.CornerImage;
                        break;
                    case BatchViewMode.Segment:
                        imageBox1.Image = batch.SegmentImage;
                        break;
                    default:
                        imageBox1.Image = batch.SegmentImage;
                        break;
                }
            }
            else
            {
                var orderedComparisons = _analysis.Comparisons.OrderBy(c => c.Distance).ToArray();
                var comparison = orderedComparisons[_view.ComparisonIndex];

                areaLabel.Text = $"Distance: {comparison.Distance}";
                imageBox1.Image = comparison.Image.Bitmap;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            imageBox1.Image.Save(DataDir.Contours("SavedFromForms.bmp"), ImageFormat.Bmp);
        }

        public class View
        {
            public ViewMode Mode { get; set; }
            public BatchViewMode BatchMode { get; set; }
            public int BatchIndex { get; set; }
            public int ComparisonIndex { get; set; }
        }

        public enum ViewMode
        {
            Batch = 1,
            Comparison = 2
        }

        public enum BatchViewMode
        {
            Source,
            Contour,
            Corner,
            Segment
        }

        private void previousButton_Click(object sender, EventArgs e)
        {
            UpdateViewIndex(false);
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            UpdateViewIndex(true);
        }

        private int CalcIndex(int count, int current, bool next)
        {
            var nextIdx = current + (next ? 1 : -1);
            if (nextIdx < 0) nextIdx = count - 1;
            if (nextIdx > count - 1) nextIdx = 0;
            return nextIdx;
        }

        private void batchMode_CheckedChanged(object sender, EventArgs e)
        {
            UpdateViewModes();
        }

        private void comparisonMode_CheckedChanged(object sender, EventArgs e)
        {
            UpdateViewModes();
        }

        private void sourceBatchViewMode_CheckedChanged(object sender, EventArgs e)
        {
            UpdateViewModes();
        }

        private void contourBatchViewMode_CheckedChanged(object sender, EventArgs e)
        {
            UpdateViewModes();
        }

        private void cornerBatchViewMode_CheckedChanged(object sender, EventArgs e)
        {
            UpdateViewModes();
        }

        private void cornerThresholdTextBox_Leave(object sender, EventArgs e)
        {
            UpdateOptions();
        }

        private void segmentBatchViewMode_CheckedChanged(object sender, EventArgs e)
        {
            UpdateOptions();
        }

        private void UpdateOptions()
        {
            if (double.TryParse(cornerThresholdTextBox.Text, out var cornerThreshold))
            {
                if (Math.Abs(_options.CornerThreshold - cornerThreshold) < 0.001) return;

                _options.CornerThreshold = cornerThreshold;
            }
            else
            {
                cornerThresholdTextBox.Text = _options.CornerThreshold.ToString("F3");
            }

            Recalculate();
        }
    }
}
