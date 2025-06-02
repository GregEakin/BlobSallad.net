using ApprovalTests.Reporters;
using ApprovalTests.Wpf;
using BlobSallad;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Xunit;
using Environment = BlobSallad.Environment;

namespace BlobSalladTests;

[UseReporter(typeof(DiffReporter), typeof(ClipboardReporter))]
public class BlobTests
{
    [Fact]
    public void CtorTest()
    {
        var blob = new Blob(71.0, 67.0, 11.0, 5);
        Assert.Equal(71.0, blob.X);
        Assert.Equal(67.0, blob.Y);
        Assert.Equal(11.0, blob.Radius);
        Assert.Equal(1.0, blob.Mass);
    }

    [WpfFact]
    public void CtorPointMassesTest()
    {
        var canvas = new Canvas { Width = 100, Height = 100 };

        var blob = new Blob(41.0, 43.0, 23.0, 5);
        var pointMasses = blob.Points;
        foreach (var pointMass in pointMasses)
            DrawDot(canvas, Brushes.Black, pointMass.Mass, pointMass.XPos, pointMass.YPos);

        var wpf = new ContentControl { Content = canvas };
        WpfApprovals.Verify(wpf);
    }

    [WpfFact]
    public void CtorSticksTest()
    {
        var canvas = new Canvas { Width = 100, Height = 100 };

        var blob = new Blob(41.0, 43.0, 23.0, 5);
        var sticks = blob.Skins;
        foreach (var skin in sticks)
            skin.Draw(canvas, 1.0);

        var wpf = new ContentControl { Content = canvas };
        WpfApprovals.Verify(wpf);
    }

    [WpfFact]
    public void CtorJointsTest()
    {
        var canvas = new Canvas { Width = 100, Height = 100 };

        var blob = new Blob(41.0, 43.0, 23.0, 5);
        DrawDot(canvas, Brushes.Blue, blob.Mass, blob.X, blob.Y);

        foreach (var bone in blob.Bones)
        {
            DrawDot(canvas, Brushes.Red, bone.PointMassA.Mass, bone.PointMassA.XPos, bone.PointMassA.YPos);
            DrawLine(canvas, Brushes.Black,
                bone.PointMassA.XPos, bone.PointMassA.YPos,
                bone.PointMassB.XPos, bone.PointMassB.YPos);
        }

        var wpf = new ContentControl { Content = canvas };
        WpfApprovals.Verify(wpf);
    }

    private static void DrawDot(Panel canvas, Brush brush, double radius, double x, double y)
    {
        var circle = new Ellipse
        {
            Width = 2.0 * radius,
            Height = 2.0 * radius,
            Fill = brush,
            Stroke = brush,
            StrokeThickness = 1.0,
            // RenderTransform = translateTransform,
        };

        Canvas.SetLeft(circle, x - radius);
        Canvas.SetTop(circle, y - radius);

        canvas.Children.Add(circle);
    }

    private static void DrawLine(Panel canvas, Brush brush, double x1, double y1, double x2, double y2)
    {
        var startPoint = new System.Windows.Point(x1, y1);
        var pathFigure = new PathFigure { StartPoint = startPoint };

        var point = new System.Windows.Point(x2, y2);
        var lineSegment1A = new LineSegment { Point = point };
        pathFigure.Segments.Add(lineSegment1A);

        var pathFigureCollection = new PathFigureCollection { pathFigure };
        var pathGeometry = new PathGeometry { Figures = pathFigureCollection };

        var path = new Path
        {
            Stroke = brush,
            StrokeThickness = 1.0,
            Data = pathGeometry,
            // RenderTransform = translateTransform
        };

        canvas.Children.Add(path);
    }

    [Fact]
    public void CtorPointMassTest()
    {
        var blob = new Blob(71.0, 67.0, 11.0, 5);
        for (var i = 0; i < 5; i++)
        {
            var pointMas = blob[i];

            var mass = i < 2 ? 4.0 : 1.0;
            Assert.Equal(mass, pointMas.Mass);

            var theta = i * 2.0 * Math.PI / 5;
            var cx = Math.Cos(theta) * 11.0 + 71.0;
            var cy = Math.Sin(theta) * 11.0 + 67.0;
            Assert.Equal(cx, pointMas.XPos);
            Assert.Equal(cy, pointMas.YPos);
        }
    }

    [Fact]
    public void AddBlob2Test()
    {
        var blob1 = new Blob(17.0, 19.0, 11.0, 3);
        var blob2 = new Blob(59.0, 61.0, 13.0, 3);
        blob1.LinkBlob(blob2);

        Assert.Equal(0, blob2.Neighbors.Length);
        Assert.Equal(1, blob1.Neighbors.Length);
        var neighbor = blob1.Neighbors[0];
        Assert.Equal(22.800, neighbor.Limit, 0.01);
    }

    [Fact]
    public void RemoveBlob2Test()
    {
        var blob1 = new Blob(17.0, 19.0, 11.0, 3);
        var blob2 = new Blob(59.0, 61.0, 13.0, 3);
        blob1.LinkBlob(blob2);
        blob1.UnLinkBlob(blob2);

        Assert.Equal(0, blob2.Neighbors.Length);
        Assert.Equal(0, blob1.Neighbors.Length);
    }

    [WpfFact]
    public void AddBlobTest()
    {
        var canvas = new Canvas { Width = 100, Height = 100 };

        var blob1 = new Blob(17.0, 19.0, 11.0, 5);
        var blob2 = new Blob(59.0, 61.0, 13.0, 5);
        blob1.LinkBlob(blob2);

        DrawDot(canvas, Brushes.Blue, blob1.Mass, blob1.X, blob1.Y);

        blob2.DrawSimpleBody(canvas, 1.0);

        foreach (var bone in blob1.Bones)
        {
            var pointMassA = bone.PointMassA;
            var pointMassB = bone.PointMassB;
            DrawDot(canvas, Brushes.Red, 2.0, pointMassA.XPos, pointMassA.YPos);
            DrawLine(canvas, Brushes.Black,
                pointMassA.XPos, pointMassA.YPos,
                pointMassB.XPos, pointMassB.YPos);
        }

        foreach (var neighbor in blob1.Neighbors)
        {
            var pointMassA = neighbor.PointMassA;
            var pointMassB = neighbor.PointMassB;
            DrawDot(canvas, Brushes.Red, 2.0, pointMassA.XPos, pointMassA.YPos);
            DrawLine(canvas, Brushes.Black,
                pointMassA.XPos, pointMassA.YPos,
                pointMassB.XPos, pointMassB.YPos);
        }

        var wpf = new ContentControl { Content = canvas };
        WpfApprovals.Verify(wpf);
    }

    [WpfFact]
    public void ScaleTest()
    {
        var canvas = new Canvas { Width = 100, Height = 100 };

        var translateTransform = new TranslateTransform(41.0, 43.0);
        var transformGroup = new TransformGroup();
        transformGroup.Children.Add(translateTransform);

        var blob = new Blob(41.0, 43.0, 23.0, 5);
        blob.Scale(3.0);
        blob.DrawSmile(canvas, 1.0, transformGroup, Brushes.Transparent);
        blob.DrawEyesOpen(canvas, 1.0, transformGroup);

        var wpf = new ContentControl { Content = canvas };
        WpfApprovals.Verify(wpf);
    }

    [WpfFact]
    public void MoveTest()
    {
        var canvas = new Canvas { Width = 100, Height = 100 };

        var blob = new Blob(41.0, 43.0, 23.0, 5) { Force = new Vector(3.0, 3.0) };
        blob.Move(2.0);

        DrawDot(canvas, Brushes.Blue, 2.0, blob.X, blob.Y);
        blob.DrawSimpleBody(canvas, 1.0);

        var wpf = new ContentControl { Content = canvas };
        WpfApprovals.Verify(wpf);
    }

    [WpfFact]
    public void ScTest()
    {
        var canvas = new Canvas { Width = 100, Height = 100 };

        var environment = new Environment(0.0, 0.0, 100.0, 100.0);
        var blob = new Blob(71.0, 67.0, 23.0, 5) { Force = new Vector(3.0, 3.0) };
        blob.Move(3.0);
        blob.Sc(environment);
        blob.DrawSimpleBody(canvas, 1.0);

        var wpf = new ContentControl { Content = canvas };
        WpfApprovals.Verify(wpf);
    }

    [Fact]
    public void SetForceTest()
    {
        // TODO:
    }

    [WpfFact]
    public void MoveToTest()
    {
        var canvas = new Canvas { Width = 100, Height = 100 };

        var blob = new Blob(41.0, 43.0, 23.0, 5);
        blob.MoveTo(61.0, 59.0);
        blob.DrawSimpleBody(canvas, 1.0);

        DrawDot(canvas, Brushes.Blue, 2.0, blob.X, blob.Y);

        var wpf = new ContentControl { Content = canvas };
        WpfApprovals.Verify(wpf);
    }

    [Fact]
    public void AddForceTest()
    {
        // TODO:
    }

    [WpfFact]
    public void DrawEyesOpenTest()
    {
        var canvas = new Canvas { Width = 100, Height = 100 };

        var translateTransform = new TranslateTransform(50.0, 50.0);
        var transformGroup = new TransformGroup();
        transformGroup.Children.Add(translateTransform);

        var blob = new Blob(50.0, 50.0, 25.0, 5);
        blob.DrawEyesOpen(canvas, 3.0, transformGroup);

        var wpf = new ContentControl { Content = canvas };
        WpfApprovals.Verify(wpf);
    }

    [WpfFact]
    public void DrawEyesClosedTest()
    {
        var canvas = new Canvas { Width = 100, Height = 100 };

        var translateTransform = new TranslateTransform(50.0, 50.0);
        var transformGroup = new TransformGroup();
        transformGroup.Children.Add(translateTransform);

        var blob = new Blob(50.0, 50.0, 25.0, 5);
        blob.DrawEyesClosed(canvas, 3.0, transformGroup);

        var wpf = new ContentControl { Content = canvas };
        WpfApprovals.Verify(wpf);
    }

    [WpfFact]
    public void DrawSmileTest()
    {
        var canvas = new Canvas { Width = 100, Height = 100 };

        var translateTransform = new TranslateTransform(50.0, 50.0);
        var transformGroup = new TransformGroup();
        transformGroup.Children.Add(translateTransform);

        var blob = new Blob(50.0, 50.0, 25.0, 5);
        blob.DrawSmile(canvas, 3.0, transformGroup, Brushes.Transparent);

        var wpf = new ContentControl { Content = canvas };
        WpfApprovals.Verify(wpf);
    }

    [WpfFact]
    public void DrawOpenMouthTest()
    {
        var canvas = new Canvas { Width = 100, Height = 100 };

        var translateTransform = new TranslateTransform(50.0, 50.0);
        var transformGroup = new TransformGroup();
        transformGroup.Children.Add(translateTransform);

        var blob = new Blob(50.0, 50.0, 25.0, 5);
        blob.DrawSmile(canvas, 3.0, transformGroup, Brushes.Black);

        var wpf = new ContentControl { Content = canvas };
        WpfApprovals.Verify(wpf);
    }

    [WpfFact]
    public void DrawOohFaceTest()
    {
        var canvas = new Canvas { Width = 100, Height = 100 };

        var translateTransform = new TranslateTransform(50.0, 50.0);
        var transformGroup = new TransformGroup();
        transformGroup.Children.Add(translateTransform);

        var blob = new Blob(50.0, 50.0, 25.0, 5);
        blob.DrawOohFace(canvas, 3.0, transformGroup);

        var wpf = new ContentControl { Content = canvas };
        WpfApprovals.Verify(wpf);
    }

    [WpfFact]
    public void DrawFaceTest()
    {
        var canvas = new Canvas { Width = 100, Height = 100 };

        var translateTransform = new TranslateTransform(50.0, 50.0);
        var transformGroup = new TransformGroup();
        transformGroup.Children.Add(translateTransform);

        var blob = new Blob(50.0, 50.0, 25.0, 5);
        blob.DrawFace(canvas, 3.0, transformGroup);

        var wpf = new ContentControl { Content = canvas };
        WpfApprovals.Verify(wpf);
    }

    [WpfFact]
    public void DrawBodyTest()
    {
        var canvas = new Canvas { Width = 100, Height = 100 };

        var blob = new Blob(50.0, 50.0, 25.0, 5);
        blob.DrawBody(canvas, 1.0);

        var wpf = new ContentControl { Content = canvas };
        WpfApprovals.Verify(wpf);
    }

    [WpfFact]
    public void DrawSimpleBodyTest()
    {
        var canvas = new Canvas { Width = 100, Height = 100 };

        var blob = new Blob(50.0, 50.0, 25.0, 5);
        blob.DrawSimpleBody(canvas, 1.0);

        var wpf = new ContentControl { Content = canvas };
        WpfApprovals.Verify(wpf);
    }

    [WpfFact]
    public void DrawTest()
    {
        var canvas = new Canvas { Width = 200, Height = 200 };

        var blob = new Blob(13.0, 17.0, 11.0, 5);
        blob.Draw(canvas, 5.0);

        var wpf = new ContentControl { Content = canvas };
        WpfApprovals.Verify(wpf);
    }
}