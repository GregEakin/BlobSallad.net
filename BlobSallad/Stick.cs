using System;

namespace BlobSallad
{
    public class Stick
    {
        private readonly PointMass pointMassA;
        private readonly PointMass pointMassB;
        private double length;
        private double lengthSquared;

        public Stick(PointMass pointMassA, PointMass pointMassB)
        {
            this.pointMassA = pointMassA;
            this.pointMassB = pointMassB;
            this.length = pointMassDist(pointMassA, pointMassB);
            this.lengthSquared = this.length * this.length;
        }

        public double getLength()
        {
            return this.length;
        }

        public double getLengthSquared()
        {
            return this.lengthSquared;
        }

        public static double pointMassDist(PointMass pointMassA, PointMass pointMassB)
        {
            double aXbX = pointMassA.getXPos() - pointMassB.getXPos();
            double aYbY = pointMassA.getYPos() - pointMassB.getYPos();
            return Math.Sqrt(aXbX * aXbX + aYbY * aYbY);
        }

        public PointMass getPointMassA()
        {
            return this.pointMassA;
        }

        public PointMass getPointMassB()
        {
            return this.pointMassB;
        }

        public void scale(double scaleFactor)
        {
            this.length *= scaleFactor;
            this.lengthSquared = this.length * this.length;
        }

        public void sc(Environment env)
        {
            Vector pointMassAPos = this.pointMassA.getPos();
            Vector pointMassBPos = this.pointMassB.getPos();

            Vector delta = new Vector(pointMassBPos);
            delta.sub(pointMassAPos);
            double dotProd = delta.dotProd(delta);
            double scaleFactor = this.lengthSquared / (dotProd + this.lengthSquared) - 0.5;
            delta.scale(scaleFactor);
            pointMassAPos.sub(delta);
            pointMassBPos.add(delta);
        }

        public void setForce(Vector force)
        {
            this.pointMassA.setForce(force);
            this.pointMassB.setForce(force);
        }

        public void addForce(Vector force)
        {
            this.pointMassA.addForce(force);
            this.pointMassB.addForce(force);
        }

        public void move(double dt)
        {
            this.pointMassA.move(dt);
            this.pointMassB.move(dt);
        }

        //public void draw(Graphics graphics, double scaleFactor)
        //{
        //    this.pointMassA.draw(graphics, scaleFactor);
        //    this.pointMassB.draw(graphics, scaleFactor);
        //    BasicStroke stroke = new BasicStroke(3.0F);
        //    Graphics2D g2d = (Graphics2D)graphics;
        //    g2d.setColor(Color.BLACK);
        //    g2d.setStroke(stroke);
        //    GeneralPath generalPath = new GeneralPath();
        //    generalPath.moveTo(this.pointMassA.getXPos() * scaleFactor, this.pointMassA.getYPos() * scaleFactor);
        //    generalPath.lineTo(this.pointMassB.getXPos() * scaleFactor, this.pointMassB.getYPos() * scaleFactor);
        //    g2d.draw(generalPath);
        //}
    }
}