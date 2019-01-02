using System;
using System.Collections.Generic;

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

            for (int i = 0; i < numPointMasses; ++i)
            {
                double theta = (double) i * 2.0 * Math.PI / (double) numPointMasses;
                double cx = Math.Cos(theta) * radius + x;
                double cy = Math.Sin(theta) * radius + y;
                double mass = i < 2 ? 4.0 : 1.0;
                PointMass pointMass = new PointMass(cx, cy, mass);
                this.pointMasses.Add(pointMass);
            }

            this.middlePointMass = new PointMass(x, y, 1.0);

            for (int i = 0; i < numPointMasses; ++i)
            {
                PointMass pointMassA = this.pointMasses[i];
                int index = (i + 1) % numPointMasses;
                PointMass pointMassB = this.pointMasses[index];
                Stick stick = new Stick(pointMassA, pointMassB);
                this.sticks.Add(stick);
            }

            double low = 0.95;
            double high = 1.05;
            for (int i = 0; i < numPointMasses; ++i)
            {
                PointMass pointMassA = this.pointMasses[i];
                int index = (i + numPointMasses / 2 + 1) % numPointMasses;
                PointMass pointMassB = this.pointMasses[index];
                Joint joint1 = new Joint(pointMassA, pointMassB, low, high);
                this.joints.Add(joint1);
                Joint joint2 = new Joint(pointMassA, this.middlePointMass, high * 0.9, low * 1.1);
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
            double dist = this.radius + blob.getRadius();
            Joint joint = new Joint(this.middlePointMass, blob.getMiddlePointMass(), 0.0, 0.0);
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
            for (int j = 0; j < 4; ++j)
            {
                foreach (var pointMass in pointMasses)
                {
                    bool collision = env.collision(pointMass.getPos(), pointMass.getPrevPos());
                    double friction = collision ? 0.75 : 0.01;
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
            PointMass pointMass1 = this.pointMasses[0];
            pointMass1.addForce(force);
            pointMass1.addForce(force);
            pointMass1.addForce(force);
            pointMass1.addForce(force);
        }

        public void moveTo(double x, double y)
        {
            Vector blobPos = this.middlePointMass.getPos();
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

        //public void drawEars(Graphics graphics, double scaleFactor)
        //{
        //}

        //public void drawEyesOpen(Graphics graphics, double scaleFactor)
        //{
        //    scaleFactor *= this.radius;
        //    BasicStroke stroke = new BasicStroke(1.0F);
        //    Graphics2D g2d = (Graphics2D)graphics;
        //    g2d.setStroke(stroke);
        //    Double arc = new Double();
        //    arc.setArcByCenter(-0.15 * scaleFactor, -0.2 * scaleFactor, 0.12 * scaleFactor, 0.0, -360.0, 1);
        //    g2d.setColor(Color.WHITE);
        //    g2d.fill(arc);
        //    g2d.setColor(Color.BLACK);
        //    g2d.draw(arc);
        //    arc.setArcByCenter(0.15 * scaleFactor, -0.2 * scaleFactor, 0.12 * scaleFactor, 0.0, -360.0, 1);
        //    g2d.setColor(Color.WHITE);
        //    g2d.fill(arc);
        //    g2d.setColor(Color.BLACK);
        //    g2d.draw(arc);
        //    g2d.setColor(Color.BLACK);
        //    arc.setArcByCenter(-0.15 * scaleFactor, -0.17 * scaleFactor, 0.06 * scaleFactor, 0.0, -360.0, 1);
        //    g2d.fill(arc);
        //    arc.setArcByCenter(0.15 * scaleFactor, -0.17 * scaleFactor, 0.06 * scaleFactor, 0.0, -360.0, 1);
        //    g2d.fill(arc);
        //}

        //public void drawEyesClosed(Graphics graphics, double scaleFactor)
        //{
        //    scaleFactor *= this.radius;
        //    BasicStroke stroke = new BasicStroke(1.0F);
        //    Graphics2D g2d = (Graphics2D)graphics;
        //    g2d.setColor(Color.BLACK);
        //    g2d.setStroke(stroke);
        //    Double arc = new Double();
        //    arc.setArcByCenter(-0.15 * scaleFactor, -0.2 * scaleFactor, 0.12 * scaleFactor, 0.0, -360.0, 0);
        //    g2d.draw(arc);
        //    arc.setArcByCenter(0.15 * scaleFactor, -0.2 * scaleFactor, 0.12 * scaleFactor, 0.0, -360.0, 0);
        //    g2d.draw(arc);
        //    GeneralPath generalPath = new GeneralPath();
        //    generalPath.moveTo(-0.25 * scaleFactor, -0.2 * scaleFactor);
        //    generalPath.lineTo(-0.05 * scaleFactor, -0.2 * scaleFactor);
        //    g2d.draw(generalPath);
        //    generalPath.reset();
        //    generalPath.moveTo(0.25 * scaleFactor, -0.2 * scaleFactor);
        //    generalPath.lineTo(0.05 * scaleFactor, -0.2 * scaleFactor);
        //    g2d.draw(generalPath);
        //}

        //public void drawSmile(Graphics graphics, double scaleFactor)
        //{
        //    scaleFactor *= this.radius;
        //    BasicStroke stroke = new BasicStroke(2.0F);
        //    Graphics2D g2d = (Graphics2D)graphics;
        //    g2d.setColor(Color.BLACK);
        //    g2d.setStroke(stroke);
        //    Double arc = new Double();
        //    arc.setArcByCenter(0.0, 0.0, 0.25 * scaleFactor, 0.0, -180.0, 0);
        //    g2d.draw(arc);
        //}

        //public void drawOpenMouth(Graphics graphics, double scaleFactor)
        //{
        //    scaleFactor *= this.radius;
        //    BasicStroke stroke = new BasicStroke(2.0F);
        //    Graphics2D g2d = (Graphics2D)graphics;
        //    g2d.setColor(Color.BLACK);
        //    g2d.setStroke(stroke);
        //    Double arc = new Double();
        //    arc.setArcByCenter(0.0, 0.0, 0.25 * scaleFactor, 0.0, -180.0, 1);
        //    g2d.fill(arc);
        //}

        //public void drawOohFace(Graphics graphics, double scaleFactor)
        //{
        //    scaleFactor *= this.radius;
        //    BasicStroke stroke = new BasicStroke(2.0F);
        //    Graphics2D g2d = (Graphics2D)graphics;
        //    g2d.setColor(Color.BLACK);
        //    g2d.setStroke(stroke);
        //    Double arc = new Double();
        //    arc.setArcByCenter(0.0, 0.1 * scaleFactor, 0.25 * scaleFactor, 0.0, -180.0, 1);
        //    g2d.fill(arc);
        //    GeneralPath generalPath = new GeneralPath();
        //    generalPath.moveTo(-0.25 * scaleFactor, -0.3 * scaleFactor);
        //    generalPath.lineTo(-0.05 * scaleFactor, -0.2 * scaleFactor);
        //    generalPath.lineTo(-0.25 * scaleFactor, -0.1 * scaleFactor);
        //    generalPath.moveTo(0.25 * scaleFactor, -0.3 * scaleFactor);
        //    generalPath.lineTo(0.05 * scaleFactor, -0.2 * scaleFactor);
        //    generalPath.lineTo(0.25 * scaleFactor, -0.1 * scaleFactor);
        //    g2d.draw(generalPath);
        //}

        //public void updateFace()
        //{
        //    if (this.drawFaceStyle == Face.SMILE && Math.random() < 0.05)
        //    {
        //        this.drawFaceStyle = Face.OPEN;
        //    }
        //    else if (this.drawFaceStyle == Face.OPEN && Math.random() < 0.1)
        //    {
        //        this.drawFaceStyle = Face.SMILE;
        //    }

        //    if (this.drawEyeStyle == Eye.OPEN && Math.random() < 0.025)
        //    {
        //        this.drawEyeStyle = Eye.CLOSED;
        //    }
        //    else if (this.drawEyeStyle == Eye.CLOSED && Math.random() < 0.3)
        //    {
        //        this.drawEyeStyle = Eye.OPEN;
        //    }
        //}

        //public void drawFace(Graphics graphics, double scaleFactor)
        //{
        //    if (this.middlePointMass.getVelocity() > 0.004)
        //    {
        //        this.drawOohFace(graphics, scaleFactor);
        //    }
        //    else
        //    {
        //        if (this.drawFaceStyle == Face.SMILE)
        //        {
        //            this.drawSmile(graphics, scaleFactor);
        //        }
        //        else
        //        {
        //            this.drawOpenMouth(graphics, scaleFactor);
        //        }

        //        if (this.drawEyeStyle == Eye.OPEN)
        //        {
        //            this.drawEyesOpen(graphics, scaleFactor);
        //        }
        //        else
        //        {
        //            this.drawEyesClosed(graphics, scaleFactor);
        //        }
        //    }
        //}

        public PointMass getPointMass(int index)
        {
            index %= this.pointMasses.Count;
            return this.pointMasses[index];
        }

        //public void drawBody(Graphics graphics, double scaleFactor)
        //{
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
        //}

        //public void drawSimpleBody(Graphics graphics, double scaleFactor)
        //{
        //    for (Stick stick : this.sticks)
        //        stick.draw(graphics, scaleFactor);

        //    for (PointMass pointMass : this.pointMasses)
        //        pointMass.draw(graphics, scaleFactor);
        //}

        //public void draw(Graphics graphics, double scaleFactor)
        //{
        //    Graphics2D g2 = (Graphics2D)graphics;
        //    this.drawBody(g2, scaleFactor);
        //    graphics.setColor(Color.WHITE);
        //    AffineTransform savedTransform = g2.getTransform();
        //    readonly double tx = this.middlePointMass.getXPos() * scaleFactor;
        //    readonly double ty = (this.middlePointMass.getYPos() - 0.35 * this.radius) * scaleFactor;
        //    g2.translate(tx, ty);
        //    Vector up = new Vector(0.0, -1.0);
        //    Vector ori = new Vector(0.0, 0.0);
        //    ori.set(this.pointMasses.get(0).getPos());
        //    ori.sub(this.middlePointMass.getPos());
        //    double ang = Math.acos(ori.dotProd(up) / ori.length());
        //    g2.rotate(ori.getX() < 0.0 ? -ang : ang);
        //    this.updateFace();
        //    this.drawFace(g2, scaleFactor);
        //    g2.setTransform(savedTransform);
        //}
    }
}