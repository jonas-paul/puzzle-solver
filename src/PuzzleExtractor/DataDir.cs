using System.IO;

namespace PuzzleExtractor
{
    public static class DataDir
    {
        public static string Contours(string filename)
        {
            return Path.GetFullPath(Path.Combine("..\\..\\..\\..\\data\\contours", filename));
        }

        public static string[] AllPiecesFiles()
        {
            var dir = Path.GetFullPath(Path.Combine("..\\..\\..\\..\\data\\allpieces"));
            return Directory.GetFiles(dir);
        }
    }
}