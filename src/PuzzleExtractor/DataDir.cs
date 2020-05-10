using System.IO;

namespace PuzzleExtractor
{
    public static class DataDir
    {
        public static string Contours(string filename)
        {
            return Path.GetFullPath(Path.Combine("..\\..\\..\\..\\data\\contours", filename));
        }
    }
}