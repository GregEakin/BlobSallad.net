// Found from https://blobsallad.se/
// Originally Written by: bjoern.lindberg@gmail.com
// Translated to C# by Greg Eakin

using System;
using System.Windows.Controls;

namespace BlobSallad;

public class Environment
{
    public Environment(double x, double y, double w, double h)
    {
        if (w < 0.0f)
            throw new Exception("Can't have negative width.");
        if (h < 0.0f)
            throw new Exception("Can't have negative height.");

        Left = x;
        Right = x + w;
        Top = y;
        Bottom = y + h;
    }

    public double Left { get; }

    public double Right { get; set; }

    public double Top { get; }

    public double Bottom { get; set; }

    public double Width
    {
        set => Right = Left + value;
    }

    public double Height
    {
        set => Bottom = Top + value;
    }

    public bool Collision(Vector curPos, Vector prePos)
    {
        var outOfBounds = false;
        if (curPos.X < Left)
        {
            curPos.X = Left;
            outOfBounds = true;
        }
        else if (curPos.X > Right)
        {
            curPos.X = Right;
            outOfBounds = true;
        }

        if (curPos.Y < Top)
        {
            curPos.Y = Top;
            outOfBounds = true;
        }
        else if (curPos.Y > Bottom)
        {
            curPos.Y = Bottom;
            outOfBounds = true;
        }

        return outOfBounds;
    }

    public void Draw(Canvas canvas, double scaleFactor)
    {
    }
}