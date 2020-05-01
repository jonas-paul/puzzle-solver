using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace PuzzleSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            //var image = Bitmap.FromFile(Desktop("singlepiece.jpg"));
            //var bitmap = new Bitmap(image);

            //var corner = bitmap.GetPixel(0, 0);
            //var upper = bitmap.GetPixel(bitmap.Width / 2, 0);
            //var center = bitmap.GetPixel(bitmap.Width / 2, bitmap.Height / 2);

            //Console.WriteLine(corner.GetBrightness());
            //Console.WriteLine(corner.GetHue());
            //Console.WriteLine(corner.GetSaturation());
            //Console.WriteLine();
            //Console.WriteLine(upper.GetBrightness());
            //Console.WriteLine(upper.GetHue());
            //Console.WriteLine(upper.GetSaturation());
            //Console.WriteLine();
            //Console.WriteLine(center.GetBrightness());
            //Console.WriteLine(center.GetHue());
            //Console.WriteLine(center.GetSaturation());

            var thresholds = new[] { 0.5f, 0.6f, 0.7f, 0.8f, 0.9f };

            foreach (var threshold in thresholds)
            {
                ThresholdImage(threshold, Desktop("multipieces.jpg"));
            }
        }

        private static void ThresholdImage(float threshold, string filepath)
        {
            var image = Image.FromFile(filepath);
            var bitmap = new Bitmap(image);

            for (var i = 0; i < image.Width; i++)
            {
                for (var j = 0; j < image.Height; j++)
                {
                    var pixel = bitmap.GetPixel(i, j);
                    var brightness = pixel.GetBrightness();

                    var color = brightness > threshold ? Color.White : Color.Black;

                    bitmap.SetPixel(i, j, color);
                }
            }

            var dir = Path.GetDirectoryName(filepath);
            var file = Path.GetFileNameWithoutExtension(filepath);

            bitmap.Save(Path.Combine(dir, $"{file}_{threshold:F2}.bmp"), ImageFormat.Bmp);
        }

        static string Desktop(string filename)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), filename);
        }
    }
}
