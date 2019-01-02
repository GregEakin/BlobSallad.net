using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace BlobSallad
{
    public class BlobCollective
    {
        private readonly int _maxNum;
        private readonly List<Blob> _blobs = new List<Blob>();
        private int _numActive = 1;
        private Blob _selectedBlob;

        public BlobCollective(double x, double y, int maxNum)
        {
            this._maxNum = maxNum;
            this._blobs.Add(new Blob(x, y, 0.4, 8));
        }

        public int GetMaxNum()
        {
            return _maxNum;
        }

        public int GetNumActive()
        {
            return _numActive;
        }

        public Blob GetSelectedBlob()
        {
            return _selectedBlob;
        }

        public void Split()
        {
            double maxRadius = 0.0;
            Blob motherBlob = null;
            if (this._numActive == this._maxNum)
                return;

            int emptySlot = this._blobs.Count;

            for (int i = 0; i < this._blobs.Count; ++i)
            {
                Blob blob = this._blobs[i];
                if (blob == null)
                {
                    emptySlot = i;
                }
                else if (blob.GetRadius() > maxRadius)
                {
                    maxRadius = blob.GetRadius();
                    motherBlob = blob;
                }
            }

            motherBlob.Scale(0.75);
            Blob newBlob = new Blob(motherBlob.GetXPos(), motherBlob.GetYPos(), motherBlob.GetRadius(), 8);

            foreach (var blob in _blobs)
            {
                if (blob == null)
                    continue;

                blob.AddBlob(newBlob);
                newBlob.AddBlob(blob);
            }

            if (emptySlot >= this._blobs.Count)
                this._blobs.Add(newBlob);
            else
                this._blobs[emptySlot] = newBlob;

            ++this._numActive;
        }

        public int FindSmallest(int exclude)
        {
            double minRadius = 1000.0;
            int minIndex = 0;

            for (int i = 0; i < this._blobs.Count; ++i)
            {
                Blob blob = this._blobs[i];
                if (i == exclude || blob == null || blob.GetRadius() >= minRadius)
                    continue;

                minIndex = i;
                minRadius = blob.GetRadius();
            }

            return minIndex;
        }

        public int FindClosest(int exclude)
        {
            double minDist = 1000.0;
            int foundIndex = 0;
            PointMass myPointMass = this._blobs[exclude].GetMiddlePointMass();

            for (int i = 0; i < this._blobs.Count; ++i)
            {
                Blob blob = this._blobs[i];
                if (i == exclude || blob == null)
                    continue;

                PointMass otherPointMass = blob.GetMiddlePointMass();
                double aXbX = myPointMass.GetXPos() - otherPointMass.GetXPos();
                double aYbY = myPointMass.GetYPos() - otherPointMass.GetYPos();
                double dist = aXbX * aXbX + aYbY * aYbY;
                if (dist >= minDist)
                    continue;

                minDist = dist;
                foundIndex = i;
            }

            return foundIndex;
        }

        public void Join()
        {
            if (this._numActive <= 1)
                return;

            int blob1Index = this.FindSmallest(-1);
            int blob2Index = this.FindClosest(blob1Index);
            double r1 = this._blobs[blob1Index].GetRadius();
            double r2 = this._blobs[blob2Index].GetRadius();
            double r3 = Math.Sqrt(r1 * r1 + r2 * r2);
            this._blobs[blob1Index] = null;
            this._blobs[blob2Index].Scale(0.945 * r3 / r2);
            --this._numActive;
        }

        public Point SelectBlob(double x, double y)
        {
            if (this._selectedBlob != null)
                return null;

            double minDist = Double.MaxValue;
            Point selectOffset = null;

            foreach (var blob in _blobs)
            {
                if (blob == null)
                    continue;

                PointMass otherPointMass = blob.GetMiddlePointMass();
                double aXbX = x - otherPointMass.GetXPos();
                double aYbY = y - otherPointMass.GetYPos();
                double dist = aXbX * aXbX + aYbY * aYbY;
                if (dist >= minDist)
                    continue;

                minDist = dist;
                if (dist >= blob.GetRadius() / 2.0)
                    continue;

                this._selectedBlob = blob;
                selectOffset = new Point(aXbX, aYbY);
            }

            if (this._selectedBlob != null)
                this._selectedBlob.SetSelected(true);

            return selectOffset;
        }

        public void UnselectBlob()
        {
            if (this._selectedBlob == null)
                return;

            this._selectedBlob.SetSelected(false);
            this._selectedBlob = null;
        }

        public void SelectedBlobMoveTo(double x, double y)
        {
            if (this._selectedBlob == null)
                return;

            this._selectedBlob.MoveTo(x, y);
        }

        public void Move(double dt)
        {
            foreach (var blob in _blobs)
            {
                if (blob == null)
                    continue;

                blob.Move(dt);
            }
        }

        public void Sc(Environment env)
        {
            foreach (var blob in _blobs)
            {
                if (blob == null)
                    continue;

                blob.Sc(env);
            }
        }

        public void SetForce(Vector force)
        {
            foreach (var blob in _blobs)
            {
                if (blob == null)
                    continue;

                Vector force1 = blob == this._selectedBlob
                    ? new Vector(0.0, 0.0)
                    : force;
                blob.SetForce(force1);
            }
        }

        private readonly Random _random = new Random();

        public void AddForce(Vector force)
        {
            foreach (var blob in _blobs)
            {
                if (blob == null || blob == this._selectedBlob)
                    continue;

                Vector tmpForce = new Vector(0.0, 0.0);
                tmpForce.SetX(force.GetX() * (_random.NextDouble() * 0.75 + 0.25));
                tmpForce.SetY(force.GetY() * (_random.NextDouble() * 0.75 + 0.25));
                blob.AddForce(tmpForce);
            }
        }

        public void Draw(Canvas canvas, double scaleFactor)
        {
            foreach (var blob in _blobs)
            {
                if (blob == null)
                    continue;

                blob.Draw(canvas, scaleFactor);
            }
        }
    }
}