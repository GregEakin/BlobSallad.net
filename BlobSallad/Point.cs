// Found from https://blobsallad.se/
// Originally Written by: bjoern.lindberg@gmail.com
// Translated to C# by Greg Eakin

namespace BlobSallad
{
    public class Point
    {
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }

        public double Y { get; set; }
    }
}