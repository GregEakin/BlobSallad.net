using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace BlobSallad
{
    public class BlobCollective
    {
        private readonly int maxNum;
        private readonly List<Blob> blobs = new List<Blob>();
        private int numActive = 1;
        private Blob selectedBlob;

        public BlobCollective(double x, double y, int maxNum)
        {
            this.maxNum = maxNum;
            this.blobs.Add(new Blob(x, y, 0.4, 8));
        }

        public int getMaxNum()
        {
            return maxNum;
        }

        public int getNumActive()
        {
            return numActive;
        }

        public Blob getSelectedBlob()
        {
            return selectedBlob;
        }

        public void split()
        {
            double maxRadius = 0.0;
            Blob motherBlob = null;
            if (this.numActive == this.maxNum)
                return;

            int emptySlot = this.blobs.Count;

            for (int i = 0; i < this.blobs.Count; ++i)
            {
                Blob blob = this.blobs[i];
                if (blob == null)
                {
                    emptySlot = i;
                }
                else if (blob.getRadius() > maxRadius)
                {
                    maxRadius = blob.getRadius();
                    motherBlob = blob;
                }
            }

            motherBlob.scale(0.75);
            Blob newBlob = new Blob(motherBlob.getXPos(), motherBlob.getYPos(), motherBlob.getRadius(), 8);

            foreach (var blob in blobs)
            {
                if (blob == null)
                    continue;

                blob.addBlob(newBlob);
                newBlob.addBlob(blob);
            }

            if (emptySlot >= this.blobs.Count)
                this.blobs.Add(newBlob);
            else
                this.blobs[emptySlot] = newBlob;

            ++this.numActive;
        }

        public int findSmallest(int exclude)
        {
            double minRadius = 1000.0;
            int minIndex = 0;

            for (int i = 0; i < this.blobs.Count; ++i)
            {
                Blob blob = this.blobs[i];
                if (i == exclude || blob == null || blob.getRadius() >= minRadius)
                    continue;

                minIndex = i;
                minRadius = blob.getRadius();
            }

            return minIndex;
        }

        public int findClosest(int exclude)
        {
            double minDist = 1000.0;
            int foundIndex = 0;
            PointMass myPointMass = this.blobs[exclude].getMiddlePointMass();

            for (int i = 0; i < this.blobs.Count; ++i)
            {
                Blob blob = this.blobs[i];
                if (i == exclude || blob == null)
                    continue;

                PointMass otherPointMass = blob.getMiddlePointMass();
                double aXbX = myPointMass.getXPos() - otherPointMass.getXPos();
                double aYbY = myPointMass.getYPos() - otherPointMass.getYPos();
                double dist = aXbX * aXbX + aYbY * aYbY;
                if (dist >= minDist)
                    continue;

                minDist = dist;
                foundIndex = i;
            }

            return foundIndex;
        }

        public void join()
        {
            if (this.numActive <= 1)
                return;

            int blob1Index = this.findSmallest(-1);
            int blob2Index = this.findClosest(blob1Index);
            double r1 = this.blobs[blob1Index].getRadius();
            double r2 = this.blobs[blob2Index].getRadius();
            double r3 = Math.Sqrt(r1 * r1 + r2 * r2);
            this.blobs[blob1Index] = null;
            this.blobs[blob2Index].scale(0.945 * r3 / r2);
            --this.numActive;
        }

        public Point selectBlob(double x, double y)
        {
            if (this.selectedBlob != null)
                return null;

            double minDist = Double.MaxValue;
            Point selectOffset = null;

            foreach (var blob in blobs)
            {
                if (blob == null)
                    continue;

                PointMass otherPointMass = blob.getMiddlePointMass();
                double aXbX = x - otherPointMass.getXPos();
                double aYbY = y - otherPointMass.getYPos();
                double dist = aXbX * aXbX + aYbY * aYbY;
                if (dist >= minDist)
                    continue;

                minDist = dist;
                if (dist >= blob.getRadius() / 2.0)
                    continue;

                this.selectedBlob = blob;
                selectOffset = new Point(aXbX, aYbY);
            }

            if (this.selectedBlob != null)
                this.selectedBlob.setSelected(true);

            return selectOffset;
        }

        public void unselectBlob()
        {
            if (this.selectedBlob == null)
                return;

            this.selectedBlob.setSelected(false);
            this.selectedBlob = null;
        }

        public void selectedBlobMoveTo(double x, double y)
        {
            if (this.selectedBlob == null)
                return;

            this.selectedBlob.moveTo(x, y);
        }

        public void move(double dt)
        {
            foreach (var blob in blobs)
            {
                if (blob == null)
                    continue;

                blob.move(dt);
            }
        }

        public void sc(Environment env)
        {
            foreach (var blob in blobs)
            {
                if (blob == null)
                    continue;

                blob.sc(env);
            }
        }

        public void setForce(Vector force)
        {
            foreach (var blob in blobs)
            {
                if (blob == null)
                    continue;

                Vector force1 = blob == this.selectedBlob
                    ? new Vector(0.0, 0.0)
                    : force;
                blob.setForce(force1);
            }
        }

        private readonly Random random = new Random();

        public void addForce(Vector force)
        {
            foreach (var blob in blobs)
            {
                if (blob == null || blob == this.selectedBlob)
                    continue;

                Vector tmpForce = new Vector(0.0, 0.0);
                tmpForce.setX(force.getX() * (random.NextDouble() * 0.75 + 0.25));
                tmpForce.setY(force.getY() * (random.NextDouble() * 0.75 + 0.25));
                blob.addForce(tmpForce);
            }
        }

        public void draw(Canvas canvas, double scaleFactor)
        {
            foreach (var blob in blobs)
            {
                if (blob == null)
                    continue;

                blob.draw(canvas, scaleFactor);
            }
        }
    }
}