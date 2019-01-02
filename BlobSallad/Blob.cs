using System;
using System.Collections.Generic;
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
            OPEN,
            CLOSED,
            CROSSED
        }

        public enum Face
        {
            SMILE,
            OPEN,
            OOH
        }

        private readonly List<Stick> sticks = new List<Stick>();
        private readonly List<PointMass> pointMasses = new List<PointMass>();

        private readonly List<Joint> joints = new List<Joint>();
        private readonly Random _random = new Random();

        //private readonly Color highlight = new Color(255, 204, 204);
        //private readonly Color normal = Color.WHITE;
        private PointMass middlePointMass;
        private double x;
        private double y;
        private double radius;
        private Face drawFaceStyle = Face.SMILE;
        private Eye drawEyeStyle = Eye.OPEN;
        private bool selected;

        public Blob(double x, double y, double radius, int numPointMasses)
        {
            if (x < 0.0 || y < 0.0)
                throw new ArgumentException("Can't have negative offsets for X and Y.");
            if (radius <= 0.0)
                throw new ArgumentException("Can't a a radius <= zero.");
            if (numPointMasses < 2)
                throw new ArgumentException("Not enough point masses.");

            this.x = x;
            this.y = y;
            this.radius = radius;

            for (var i = 0; i < numPointMasses; ++i)
            {
                var theta = (double) i * 2.0 * Math.PI / (double) numPointMasses;
                var cx = Math.Cos(theta) * radius + x;
                var cy = Math.Sin(theta) * radius + y;
                var mass = i < 2 ? 4.0 : 1.0;
                var pointMass = new PointMass(cx, cy, mass);
                this.pointMasses.Add(pointMass);
            }

            this.middlePointMass = new PointMass(x, y, 1.0);

            for (var i = 0; i < numPointMasses; ++i)
            {
                var pointMassA = this.pointMasses[i];
                var index = (i + 1) % numPointMasses;
                var pointMassB = this.pointMasses[index];
                var stick = new Stick(pointMassA, pointMassB);
                this.sticks.Add(stick);
            }

            var low = 0.95;
            var high = 1.05;
            for (var i = 0; i < numPointMasses; ++i)
            {
                var pointMassA = this.pointMasses[i];
                var index = (i + numPointMasses / 2 + 1) % numPointMasses;
                var pointMassB = this.pointMasses[index];
                var joint1 = new Joint(pointMassA, pointMassB, low, high);
                this.joints.Add(joint1);
                var joint2 = new Joint(pointMassA, this.middlePointMass, high * 0.9, low * 1.1);
                this.joints.Add(joint2);
            }
        }

        public PointMass[] getPointMasses()
        {
            return pointMasses.ToArray();
        }

        public Stick[] getSticks()
        {
            return sticks.ToArray();
        }

        public Joint[] getJoints()
        {
            return joints.ToArray();
        }

        public PointMass getMiddlePointMass()
        {
            return this.middlePointMass;
        }

        public double getRadius()
        {
            return this.radius;
        }

        public void addBlob(Blob blob)
        {
            var dist = this.radius + blob.getRadius();
            var joint = new Joint(this.middlePointMass, blob.getMiddlePointMass(), 0.0, 0.0);
            joint.setDist(dist * 0.95, 0.0);
            this.joints.Add(joint);
        }

        public double getXPos()
        {
            return this.middlePointMass.getXPos();
        }

        public double getYPos()
        {
            return this.middlePointMass.getYPos();
        }

        public void scale(double scaleFactor)
        {
            foreach (var joint in joints)
                joint.scale(scaleFactor);

            foreach (var stick in sticks)
                stick.scale(scaleFactor);

            this.radius *= scaleFactor;
        }

        public void move(double dt)
        {
            foreach (var pointMass in pointMasses)
                pointMass.move(dt);

            this.middlePointMass.move(dt);
        }

        public void sc(Environment env)
        {
            for (var j = 0; j < 4; ++j)
            {
                foreach (var pointMass in pointMasses)
                {
                    var collision = env.collision(pointMass.getPos(), pointMass.getPrevPos());
                    var friction = collision ? 0.75 : 0.01;
                    pointMass.setFriction(friction);
                }

                foreach (var stick in sticks)
                {
                    stick.sc(env);
                }

                foreach (var joint in joints)
                {
                    joint.sc();
                }
            }
        }

        public void setForce(Vector force)
        {
            foreach (var pointMass in pointMasses)
                pointMass.setForce(force);

            this.middlePointMass.setForce(force);
        }

        public void addForce(Vector force)
        {
            foreach (var pointMass in pointMasses)
                pointMass.addForce(force);

            this.middlePointMass.addForce(force);
            var pointMass1 = this.pointMasses[0];
            pointMass1.addForce(force);
            pointMass1.addForce(force);
            pointMass1.addForce(force);
            pointMass1.addForce(force);
        }

        public void moveTo(double x, double y)
        {
            var blobPos = this.middlePointMass.getPos();
            x -= blobPos.getX();
            y -= blobPos.getY();

            foreach (var pointMass in pointMasses)
            {
                blobPos = pointMass.getPos();
                blobPos.addX(x);
                blobPos.addY(y);
            }

            blobPos = this.middlePointMass.getPos();
            blobPos.addX(x);
            blobPos.addY(y);
        }

        public bool getSelected()
        {
            return this.selected;
        }

        public void setSelected(bool selected)
        {
            this.selected = selected;
        }

        public void drawEars(Canvas canvas, double scaleFactor)
        {
        }

        public void drawEyesOpen(Canvas canvas, double scaleFactor, TranslateTransform translateTransform)
        {
            {
                var circle = new Ellipse
                {
                    Width = 0.24 * radius * scaleFactor,
                    Height = 0.24 * radius * scaleFactor,
                    Fill = Brushes.White,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.0,
                    RenderTransform = translateTransform,
                };

                Canvas.SetLeft(circle, -0.27 * radius * scaleFactor);
                Canvas.SetTop(circle, -0.32 * radius * scaleFactor);

                canvas.Children.Add(circle);
            }

            {
                var circle = new Ellipse
                {
                    Width = 0.24 * radius * scaleFactor,
                    Height = 0.24 * radius * scaleFactor,
                    Fill = Brushes.White,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.0,
                    RenderTransform = translateTransform,
                };

                Canvas.SetLeft(circle, 0.03 * radius * scaleFactor);
                Canvas.SetTop(circle, -0.32 * radius * scaleFactor);

                canvas.Children.Add(circle);
            }

            {
                var circle = new Ellipse
                {
                    Width = 0.12 * radius * scaleFactor,
                    Height = 0.12 * radius * scaleFactor,
                    Fill = Brushes.Black,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.0,
                    RenderTransform = translateTransform,
                };

                Canvas.SetLeft(circle, -0.21 * radius * scaleFactor);
                Canvas.SetTop(circle, -0.23 * radius * scaleFactor);

                canvas.Children.Add(circle);
            }

            {
                var circle = new Ellipse
                {
                    Width = 0.12 * radius * scaleFactor,
                    Height = 0.12 * radius * scaleFactor,
                    Fill = Brushes.Black,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.0,
                    RenderTransform = translateTransform,
                };

                Canvas.SetLeft(circle, 0.09 * radius * scaleFactor);
                Canvas.SetTop(circle, -0.23 * radius * scaleFactor);

                canvas.Children.Add(circle);
            }
        }

        public void drawEyesClosed(Canvas canvas, double scaleFactor, TranslateTransform translateTransform)
        {
            {
                var circle = new Ellipse
                {
                    Width = 0.24 * radius * scaleFactor,
                    Height = 0.24 * radius * scaleFactor,
                    Fill = Brushes.White,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.0,
                    RenderTransform = translateTransform,
                };

                Canvas.SetLeft(circle, -0.27 * radius * scaleFactor);
                Canvas.SetTop(circle, -0.32 * radius * scaleFactor);

                canvas.Children.Add(circle);
            }

            {
                var circle = new Ellipse
                {
                    Width = 0.24 * radius * scaleFactor,
                    Height = 0.24 * radius * scaleFactor,
                    Fill = Brushes.White,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.0,
                    RenderTransform = translateTransform,
                };

                Canvas.SetLeft(circle, 0.03 * radius * scaleFactor);
                Canvas.SetTop(circle, -0.32 * radius * scaleFactor);

                canvas.Children.Add(circle);
            }

            {
                var startPointA = new System.Windows.Point(-0.25 * radius * scaleFactor, -0.2 * radius * scaleFactor);
                var pathFigureA = new PathFigure {StartPoint = startPointA};

                var pointA = new System.Windows.Point(-0.05 * radius * scaleFactor, -0.2 * radius * scaleFactor);
                var lineSegment1A = new LineSegment {Point = pointA};
                pathFigureA.Segments.Add(lineSegment1A);

                var startPointB = new System.Windows.Point(0.25 * radius * scaleFactor, -0.2 * radius * scaleFactor);
                var pathFigureB = new PathFigure {StartPoint = startPointB};

                var pointB = new System.Windows.Point(0.05 * radius * scaleFactor, -0.2 * radius * scaleFactor);
                var lineSegment1B = new LineSegment {Point = pointB};
                pathFigureB.Segments.Add(lineSegment1B);

                var pathGeometry = new PathGeometry {Figures = new PathFigureCollection {pathFigureA, pathFigureB}};

                var orangePath = new Path
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.0,
                    Data = pathGeometry,
                    RenderTransform = translateTransform
                };

                canvas.Children.Add(orangePath);
            }
        }

        public void drawSmile(Canvas canvas, double scaleFactor, TranslateTransform translateTransform)
        {
            var point = new System.Windows.Point(0.25 * radius * scaleFactor, 0.1 * radius * scaleFactor);
            var size = new Size(0.25 * radius * scaleFactor, 0.25 * radius * scaleFactor);
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
                StartPoint = new System.Windows.Point(-0.25 * radius * scaleFactor, 0.1 * radius * scaleFactor)
            };
            pathFigure.Segments.Add(arcSegment);

            var pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);

            var arcPath = new Path
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2.0,
                Data = pathGeometry,
                Fill = Brushes.Transparent,
                RenderTransform = translateTransform
            };

            canvas.Children.Add(arcPath);
        }

        public void drawOpenMouth(Canvas canvas, double scaleFactor, TranslateTransform translateTransform)
        {
            var point = new System.Windows.Point(0.25 * radius * scaleFactor, 0.1 * radius * scaleFactor);
            var size = new Size(0.25 * radius * scaleFactor, 0.25 * radius * scaleFactor);
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
                StartPoint = new System.Windows.Point(-0.25 * radius * scaleFactor, 0.1 * radius * scaleFactor)
            };
            pathFigure.Segments.Add(arcSegment);

            var pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);

            var arcPath = new Path
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2.0,
                Data = pathGeometry,
                Fill = Brushes.Black,
                RenderTransform = translateTransform
            };

            canvas.Children.Add(arcPath);
        }

        public void drawOohFace(Canvas canvas, double scaleFactor, TranslateTransform translateTransform)
        {
            {
                var point = new System.Windows.Point(0.25 * radius * scaleFactor, 0.1 * radius * scaleFactor);
                var size = new Size(0.25 * radius * scaleFactor, 0.25 * radius * scaleFactor);
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
                    StartPoint = new System.Windows.Point(-0.25 * radius * scaleFactor, 0.1 * radius * scaleFactor)
                };
                pathFigure.Segments.Add(arcSegment);

                var pathGeometry = new PathGeometry();
                pathGeometry.Figures.Add(pathFigure);

                var arcPath = new Path
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 2.0,
                    Data = pathGeometry,
                    Fill = Brushes.Black,
                    RenderTransform = translateTransform
                };

                canvas.Children.Add(arcPath);
            }

            {
                var startPointA = new System.Windows.Point(-0.25 * radius * scaleFactor, -0.3 * radius * scaleFactor);
                var pathFigureA = new PathFigure {StartPoint = startPointA};

                var point1A = new System.Windows.Point(-0.05 * radius * scaleFactor, -0.2 * radius * scaleFactor);
                var lineSegment1A = new LineSegment {Point = point1A};
                pathFigureA.Segments.Add(lineSegment1A);

                var point2A = new System.Windows.Point(-0.25 * radius * scaleFactor, -0.1 * radius * scaleFactor);
                var lineSegment2A = new LineSegment {Point = point2A};
                pathFigureA.Segments.Add(lineSegment2A);

                var startPointB = new System.Windows.Point(0.25 * radius * scaleFactor, -0.3 * radius * scaleFactor);
                var pathFigureB = new PathFigure {StartPoint = startPointB};

                var point1B = new System.Windows.Point(0.05 * radius * scaleFactor, -0.2 * radius * scaleFactor);
                var lineSegment1B = new LineSegment {Point = point1B};
                pathFigureB.Segments.Add(lineSegment1B);

                var point2B = new System.Windows.Point(0.25 * radius * scaleFactor, -0.1 * radius * scaleFactor);
                var lineSegment2B = new LineSegment {Point = point2B};
                pathFigureB.Segments.Add(lineSegment2B);

                // Figures = {M-34.5,-41.4L-6.9,-27.6L-34.5,-13.8 M34.5,-41.4L6.9,-27.6L34.5,-13.8}
                var pathGeometry = new PathGeometry {Figures = new PathFigureCollection {pathFigureA, pathFigureB}};

                var orangePath = new Path
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 2.0,
                    Data = pathGeometry,
                    RenderTransform = translateTransform
                };

                canvas.Children.Add(orangePath);
            }
        }

        public void updateFace()
        {
            if (this.drawFaceStyle == Face.SMILE && _random.NextDouble() < 0.05)
            {
                this.drawFaceStyle = Face.OPEN;
            }
            else if (this.drawFaceStyle == Face.OPEN && _random.NextDouble() < 0.1)
            {
                this.drawFaceStyle = Face.SMILE;
            }

            if (this.drawEyeStyle == Eye.OPEN && _random.NextDouble() < 0.025)
            {
                this.drawEyeStyle = Eye.CLOSED;
            }
            else if (this.drawEyeStyle == Eye.CLOSED && _random.NextDouble() < 0.3)
            {
                this.drawEyeStyle = Eye.OPEN;
            }
        }

        public void drawFace(Canvas canvas, double scaleFactor, TranslateTransform translateTransform)
        {
            if (this.middlePointMass.getVelocity() > 0.004)
            {
                this.drawOohFace(canvas, scaleFactor, translateTransform);
            }
            else
            {
                if (this.drawFaceStyle == Face.SMILE)
                {
                    this.drawSmile(canvas, scaleFactor, translateTransform);
                }
                else
                {
                    this.drawOpenMouth(canvas, scaleFactor, translateTransform);
                }

                if (this.drawEyeStyle == Eye.OPEN)
                {
                    this.drawEyesOpen(canvas, scaleFactor, translateTransform);
                }
                else
                {
                    this.drawEyesClosed(canvas, scaleFactor, translateTransform);
                }
            }
        }

        public PointMass getPointMass(int index)
        {
            index %= this.pointMasses.Count;
            return this.pointMasses[index];
        }

        public void drawBody(Canvas canvas, double scaleFactor)
        {
            //    GeneralPath generalPath = new GeneralPath();
            //    generalPath.moveTo(this.pointMasses.get(0).getXPos() * scaleFactor, this.pointMasses.get(0).getYPos() * scaleFactor);

            //    for (int i = 0; i < this.pointMasses.size(); ++i)
            //    {
            //        PointMass prevPointMass = this.getPointMass(i - 1);
            //        PointMass currentPointMass = this.pointMasses.get(i);
            //        PointMass nextPointMass = this.getPointMass(i + 1);
            //        PointMass nextNextPointMass = this.getPointMass(i + 2);
            //        double tx = nextPointMass.getXPos();
            //        double ty = nextPointMass.getYPos();
            //        double cx = currentPointMass.getXPos();
            //        double cy = currentPointMass.getYPos();
            //        double px = cx * 0.5 + tx * 0.5;
            //        double py = cy * 0.5 + ty * 0.5;
            //        double nx = cx - prevPointMass.getXPos() + tx - nextNextPointMass.getXPos();
            //        double ny = cy - prevPointMass.getYPos() + ty - nextNextPointMass.getYPos();
            //        px += nx * 0.16;
            //        py += ny * 0.16;
            //        px *= scaleFactor;
            //        py *= scaleFactor;
            //        tx *= scaleFactor;
            //        ty *= scaleFactor;
            //        generalPath.curveTo(px, py, tx, ty, tx, ty);
            //    }

            //    generalPath.closePath();
            //    BasicStroke stroke = new BasicStroke(5.0F);
            //    Color color = this.selected ? highlight : normal;

            //    Graphics2D g2d = (Graphics2D)graphics;
            //    g2d.setColor(Color.BLACK);
            //    g2d.setStroke(stroke);
            //    g2d.draw(generalPath);
            //    g2d.setColor(color);
            //    g2d.fill(generalPath);
        }

        public void drawSimpleBody(Canvas canvas, double scaleFactor)
        {
            foreach (var stick in sticks)
                stick.draw(canvas, scaleFactor);

            foreach (var pointMass in pointMasses)
                pointMass.draw(canvas, scaleFactor);
        }

        public void draw(Canvas canvas, double scaleFactor)
        {
            drawBody(canvas, scaleFactor);
            //    graphics.setColor(Color.WHITE);
            //    AffineTransform savedTransform = g2.getTransform();
            var tx = this.middlePointMass.getXPos() * scaleFactor;
            var ty = (this.middlePointMass.getYPos() - 0.35 * this.radius) * scaleFactor;
            var translateTransform = new TranslateTransform(tx, ty);

            Vector up = new Vector(0.0, -1.0);
            Vector ori = new Vector(0.0, 0.0);
            ori.set(this.pointMasses[0].getPos());
            ori.sub(this.middlePointMass.getPos());
            double ang = Math.Acos(ori.dotProd(up) / ori.length());
            //    g2.rotate(ori.getX() < 0.0 ? -ang : ang);
            this.updateFace();
            this.drawFace(canvas, scaleFactor, translateTransform);
            //    g2.setTransform(savedTransform);
        }
    }
}