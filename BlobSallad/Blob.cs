// Found from https://blobsallad.se/
// Originally Written by: bjoern.lindberg@gmail.com
// Translated to C# by Greg Eakin

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BlobSallad
{
    public class Blob
    {
        public enum Eye
        {
            Open,
            Closed,
            Crossed
        }

        public enum Face
        {
            Smile,
            Open,
            Ooh
        }

        private readonly PointMass _middle;
        private readonly List<PointMass> _points = new List<PointMass>();
        private readonly List<Skin> _skins = new List<Skin>();
        private readonly List<Bones> _bones = new List<Bones>();
        private readonly List<Collision> _collisions = new List<Collision>();
        private readonly Random _random = new Random();
        private readonly Color _highlight = Colors.Pink; // 255, 204, 204
        private readonly Color _normal = Colors.White;

        private Face _drawFaceStyle = Face.Smile;
        private Eye _drawEyeStyle = Eye.Open;

        public Blob(double x, double y, double radius, int numPoints)
        {
            if (radius <= 0.0)
                throw new ArgumentException("Can't have a negative radius.");
            if (numPoints < 0)
                throw new ArgumentException("Not enough points.");

            Radius = radius;

            for (var i = 0; i < numPoints; ++i)
            {
                var theta = i * 2.0 * Math.PI / numPoints;
                var cx = Math.Cos(theta) * radius + x;
                var cy = Math.Sin(theta) * radius + y;
                var mass = i < 2 ? 4.0 : 1.0;
                var pointMass = new PointMass(cx, cy, mass);
                _points.Add(pointMass);
            }

            _middle = new PointMass(x, y, 1.0);

            for (var i = 0; i < numPoints; ++i)
            {
                var pointMassA = _points[i];
                var index = PointMassIndex(i + 1);
                var pointMassB = _points[index];
                var skin = new Skin(pointMassA, pointMassB);
                _skins.Add(skin);
            }

            for (var i = 0; i < numPoints; ++i)
            {
                const double crossShort = 0.95;
                const double crossLong = 1.05;
                const double middleShort = crossLong * 0.9;
                const double middleLong = crossShort * 1.1;
                var pointMassA = _points[i];

                var index = PointMassIndex(i + numPoints / 2 + 1);
                var pointMassB = _points[index];
                var bone1 = new Bones(pointMassA, pointMassB, crossShort, crossLong);
                _bones.Add(bone1);

                var bone2 = new Bones(pointMassA, _middle, middleShort, middleLong);
                _bones.Add(bone2);
            }
        }

        public Blob(Blob motherBlob)
            : this(motherBlob.X, motherBlob.Y, motherBlob.Radius, motherBlob._points.Count)
        {
        }

        public PointMass[] Points => _points.ToArray();

        public Skin[] Skins => _skins.ToArray();

        public Bones[] Bones => _bones.ToArray();

        public Collision[] Collisions => _collisions.ToArray();

        public double Radius { get; private set; }

        public bool Selected { get; set; }

        public double X => _middle.XPos;

        public double Y => _middle.YPos;

        public double Mass => _middle.Mass;

        public int PointMassIndex(int x)
        {
            var m = _points.Count;
            return (x % m + m) % m;
        }

        public void LinkBlob(Blob blob)
        {
            var dist = Radius + blob.Radius;
            var collision = new Collision(_middle, blob._middle, dist * 0.95);
            _collisions.Add(collision);
        }

        public void UnLinkBlob(Blob blob)
        {
            foreach (var collision in _collisions)
            {
                if (collision.PointMassB != blob._middle)
                    continue;

                _collisions.Remove(collision);
                break;
            }
        }

        public void Scale(double scaleFactor)
        {
            foreach (var skin in _skins)
                skin.Scale(scaleFactor);

            foreach (var bone in _bones)
                bone.Scale(scaleFactor);

            foreach (var collision in _collisions)
                collision.Scale(scaleFactor);

            Radius *= scaleFactor;
        }

        public void Move(double dt)
        {
            foreach (var pointMass in _points)
                pointMass.Move(dt);

            _middle.Move(dt);
        }

        public void Sc(Environment env)
        {
            for (var j = 0; j < 4; ++j)
            {
                foreach (var pointMass in _points)
                {
                    var collision = env.Collision(pointMass.Pos, pointMass.Prev);
                    var friction = collision ? 0.75 : 0.01;
                    pointMass.Friction = friction;
                }

                foreach (var skin in _skins)
                    skin.Sc(env);

                foreach (var bone in _bones)
                    bone.Sc(env);

                foreach (var collision in _collisions)
                    collision.Sc(env);
            }
        }

        public Vector Force
        {
            get => _middle.Force;
            set
            {
                _middle.Force = value;
                foreach (var pointMass in _points)
                    pointMass.Force = value;
            }
        }

        public void AddForce(Vector force)
        {
            _middle.AddForce(force);
            foreach (var point in _points)
                point.AddForce(force);

            if (!_points.Any())
                return;

            // put a spin on the blob
            var pointMass = _points[0];
            pointMass.AddForce(force);
            pointMass.AddForce(force);
            pointMass.AddForce(force);
            pointMass.AddForce(force);
        }

        public void MoveTo(double x, double y)
        {
            var blobPos = _middle.Pos;
            x -= blobPos.X;
            y -= blobPos.Y;

            foreach (var pointMass in _points)
            {
                blobPos = pointMass.Pos;
                blobPos.AddX(x);
                blobPos.AddY(y);
            }

            blobPos = _middle.Pos;
            blobPos.AddX(x);
            blobPos.AddY(y);
        }

        public void DrawEars(Canvas canvas, double scaleFactor)
        {
        }

        public void DrawEyesOpen(Canvas canvas, double scaleFactor, TransformGroup translateTransform)
        {
            {
                var radius = 0.12 * Radius * scaleFactor;
                var x = -0.15 * Radius * scaleFactor;
                var y = -0.20 * Radius * scaleFactor;
                var circle = new EllipseGeometry(new Point(x, y), radius, radius, translateTransform);
                var path = new Path
                {
                    Fill = Brushes.White,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.0,
                    Data = circle,
                };
                canvas.Children.Add(path);
            }

            {
                var radius = 0.12 * Radius * scaleFactor;
                var x = 0.15 * Radius * scaleFactor;
                var y = -0.20 * Radius * scaleFactor;
                var circle = new EllipseGeometry(new Point(x, y), radius, radius, translateTransform);
                var path = new Path
                {
                    Fill = Brushes.White,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.0,
                    Data = circle,
                };
                canvas.Children.Add(path);
            }

            {
                var radius = 0.06 * Radius * scaleFactor;
                var x = -0.15 * Radius * scaleFactor;
                var y = -0.17 * Radius * scaleFactor;
                var circle = new EllipseGeometry(new Point(x, y), radius, radius, translateTransform);
                var path = new Path
                {
                    Fill = Brushes.Black,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.0,
                    Data = circle,
                };
                canvas.Children.Add(path);
            }

            {
                var radius = 0.06 * Radius * scaleFactor;
                var x = 0.15 * Radius * scaleFactor;
                var y = -0.17 * Radius * scaleFactor;
                var circle = new EllipseGeometry(new Point(x, y), radius, radius, translateTransform);
                var path = new Path
                {
                    Fill = Brushes.Black,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.0,
                    Data = circle,
                };
                canvas.Children.Add(path);
            }
        }

        public void DrawEyesClosed(Canvas canvas, double scaleFactor, TransformGroup translateTransform)
        {
            {
                var radius = 0.12 * Radius * scaleFactor;
                var x = -0.15 * Radius * scaleFactor;
                var y = -0.20 * Radius * scaleFactor;
                var circle = new EllipseGeometry(new Point(x, y), radius, radius);
                var leftEye = new Path
                {
                    Fill = Brushes.White,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.0,
                    Data = circle,
                    RenderTransform = translateTransform
                };
                canvas.Children.Add(leftEye);
            }

            {
                var radius = 0.12 * Radius * scaleFactor;
                var x = 0.15 * Radius * scaleFactor;
                var y = -0.20 * Radius * scaleFactor;
                var circle = new EllipseGeometry(new Point(x, y), radius, radius);
                var rightEye = new Path
                {
                    Fill = Brushes.White,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.0,
                    Data = circle,
                    RenderTransform = translateTransform
                };
                canvas.Children.Add(rightEye);
            }

            {
                var startPointA = new Point(-0.25 * Radius * scaleFactor, -0.20 * Radius * scaleFactor);
                var pathFigureA = new PathFigure {StartPoint = startPointA};

                var pointA = new Point(-0.05 * Radius * scaleFactor, -0.20 * Radius * scaleFactor);
                var lineSegment1A = new LineSegment {Point = pointA};
                pathFigureA.Segments.Add(lineSegment1A);

                var startPointB = new Point(0.25 * Radius * scaleFactor, -0.20 * Radius * scaleFactor);
                var pathFigureB = new PathFigure {StartPoint = startPointB};

                var pointB = new Point(0.05 * Radius * scaleFactor, -0.20 * Radius * scaleFactor);
                var lineSegment1B = new LineSegment {Point = pointB};
                pathFigureB.Segments.Add(lineSegment1B);

                var pathFigureCollection = new PathFigureCollection {pathFigureA, pathFigureB};
                var pathGeometry = new PathGeometry {Figures = pathFigureCollection};
                var path = new Path
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.0,
                    Data = pathGeometry,
                    RenderTransform = translateTransform
                };

                canvas.Children.Add(path);
            }
        }

        public void DrawSmile(Canvas canvas, double scaleFactor, TransformGroup translateTransform, Brush fill)
        {
            var point = new Point(0.25 * Radius * scaleFactor, 0.1 * Radius * scaleFactor);
            var size = new Size(0.25 * Radius * scaleFactor, 0.25 * Radius * scaleFactor);
            var arcSegment = new ArcSegment
            {
                Point = point,
                Size = size,
                IsLargeArc = true,
                SweepDirection = SweepDirection.Counterclockwise,
            };

            // pathGeometry = {M-34.5,13.8A27.6,27.6,0,1,0,34.5,13.8}
            var pathFigure = new PathFigure
            {
                StartPoint = new Point(-0.25 * Radius * scaleFactor, 0.1 * Radius * scaleFactor)
            };
            pathFigure.Segments.Add(arcSegment);

            var pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);

            var arcPath = new Path
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2.0,
                Data = pathGeometry,
                Fill = fill,
                RenderTransform = translateTransform
            };

            canvas.Children.Add(arcPath);
        }

        public void DrawOohFace(Canvas canvas, double scaleFactor, TransformGroup translateTransform)
        {
            DrawSmile(canvas, scaleFactor, translateTransform, Brushes.Black);

            var startPointA = new Point(-0.25 * Radius * scaleFactor, -0.3 * Radius * scaleFactor);
            var pathFigureA = new PathFigure {StartPoint = startPointA};

            var point1A = new Point(-0.05 * Radius * scaleFactor, -0.2 * Radius * scaleFactor);
            var lineSegment1A = new LineSegment {Point = point1A};
            pathFigureA.Segments.Add(lineSegment1A);

            var point2A = new Point(-0.25 * Radius * scaleFactor, -0.1 * Radius * scaleFactor);
            var lineSegment2A = new LineSegment {Point = point2A};
            pathFigureA.Segments.Add(lineSegment2A);

            var startPointB = new Point(0.25 * Radius * scaleFactor, -0.3 * Radius * scaleFactor);
            var pathFigureB = new PathFigure {StartPoint = startPointB};

            var point1B = new Point(0.05 * Radius * scaleFactor, -0.2 * Radius * scaleFactor);
            var lineSegment1B = new LineSegment {Point = point1B};
            pathFigureB.Segments.Add(lineSegment1B);

            var point2B = new Point(0.25 * Radius * scaleFactor, -0.1 * Radius * scaleFactor);
            var lineSegment2B = new LineSegment {Point = point2B};
            pathFigureB.Segments.Add(lineSegment2B);

            // Figures = {M-34.5,-41.4L-6.9,-27.6L-34.5,-13.8 M34.5,-41.4L6.9,-27.6L34.5,-13.8}
            var pathGeometry = new PathGeometry {Figures = new PathFigureCollection {pathFigureA, pathFigureB}};
            var path = new Path
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2.0,
                Data = pathGeometry,
                RenderTransform = translateTransform
            };

            canvas.Children.Add(path);
        }

        public void UpdateFace()
        {
            if (_drawFaceStyle == Face.Smile && _random.NextDouble() < 0.05)
            {
                _drawFaceStyle = Face.Open;
            }
            else if (_drawFaceStyle == Face.Open && _random.NextDouble() < 0.1)
            {
                _drawFaceStyle = Face.Smile;
            }

            if (_drawEyeStyle == Eye.Open && _random.NextDouble() < 0.025)
            {
                _drawEyeStyle = Eye.Closed;
            }
            else if (_drawEyeStyle == Eye.Closed && _random.NextDouble() < 0.3)
            {
                _drawEyeStyle = Eye.Open;
            }
        }

        public void DrawFace(Canvas canvas, double scaleFactor, TransformGroup translateTransform)
        {
            if (_middle.Velocity > 0.004)
            {
                DrawOohFace(canvas, scaleFactor, translateTransform);
            }
            else
            {
                if (_drawFaceStyle == Face.Smile)
                {
                    DrawSmile(canvas, scaleFactor, translateTransform, Brushes.Transparent);
                }
                else
                {
                    DrawSmile(canvas, scaleFactor, translateTransform, Brushes.Black);
                }

                if (_drawEyeStyle == Eye.Open)
                {
                    DrawEyesOpen(canvas, scaleFactor, translateTransform);
                }
                else
                {
                    DrawEyesClosed(canvas, scaleFactor, translateTransform);
                }
            }
        }

        public PointMass this[int index]
        {
            get
            {
                index = PointMassIndex(index);
                return _points[index];
            }
        }

        public void DrawBody(Canvas canvas, double scaleFactor)
        {
            var pbzSeg = new PolyBezierSegment();
            for (var i = 0; i < Points.Length; ++i)
            {
                var prevPointMass = Points[PointMassIndex(i - 1)];
                var currentPointMass = Points[PointMassIndex(i)];
                var nextPointMass = Points[PointMassIndex(i + 1)];
                var nextNextPointMass = Points[PointMassIndex(i + 2)];

                var cx = currentPointMass.XPos;
                var cy = currentPointMass.YPos;
                var tx = nextPointMass.XPos;
                var ty = nextPointMass.YPos;
                var nx = cx - prevPointMass.XPos + tx - nextNextPointMass.XPos;
                var ny = cy - prevPointMass.YPos + ty - nextNextPointMass.YPos;
                var px = cx * 0.5 + tx * 0.5 + nx * 0.16;
                var py = cy * 0.5 + ty * 0.5 + ny * 0.16;

                tx *= scaleFactor;
                ty *= scaleFactor;
                px *= scaleFactor;
                py *= scaleFactor;

                // generalPath.curveTo(px, py, tx, ty, tx, ty);
                pbzSeg.Points.Add(new Point(px, py));
                pbzSeg.Points.Add(new Point(tx, ty));
                pbzSeg.Points.Add(new Point(tx, ty));
            }

            var myPathSegmentCollection = new PathSegmentCollection {pbzSeg};

            var startX = Points[0].XPos * scaleFactor;
            var startY = Points[0].YPos * scaleFactor;
            var pthFigure = new PathFigure
            {
                StartPoint = new Point(startX, startY),
                Segments = myPathSegmentCollection
            };

            var pthFigureCollection = new PathFigureCollection {pthFigure};

            var pthGeometry = new PathGeometry {Figures = pthFigureCollection};

            var color = Selected ? _highlight : _normal;

            var arcPath = new Path
            {
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 3,
                Data = pthGeometry,
                Fill = new SolidColorBrush(color)
            };

            canvas.Children.Add(arcPath);
        }

        public void DrawSimpleBody(Canvas canvas, double scaleFactor)
        {
            foreach (var stick in _skins)
                stick.Draw(canvas, scaleFactor);

            foreach (var pointMass in _points)
                pointMass.Draw(canvas, scaleFactor);
        }

        public void Draw(Canvas canvas, double scaleFactor)
        {
            var transformGroup = new TransformGroup();

            var up = new Vector(0.0, -1.0);
            var ori = _points[0].Pos - _middle.Pos;
            var ang = Math.Acos(ori.DotProd(up) / ori.Length);
            var radians = (ori.X < 0.0) ? -ang : ang;
            var theta = (180.0 / Math.PI) * radians;
            var rotateTransform = new RotateTransform(theta);
            transformGroup.Children.Add(rotateTransform);

            var tx = _middle.XPos * scaleFactor;
            var ty = (_middle.YPos - 0.35 * Radius) * scaleFactor;
            var translateTransform = new TranslateTransform(tx, ty);
            transformGroup.Children.Add(translateTransform);

            DrawBody(canvas, scaleFactor);
            UpdateFace();
            DrawFace(canvas, scaleFactor, transformGroup);
        }
    }
}