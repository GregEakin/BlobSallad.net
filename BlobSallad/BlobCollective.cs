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
            _maxNum = maxNum;
            _blobs.Add(new Blob(x, y, 0.4, 8));
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
            var maxRadius = 0.0;
            Blob motherBlob = null;
            if (_numActive == _maxNum)
                return;

            var emptySlot = _blobs.Count;

            for (var i = 0; i < _blobs.Count; ++i)
            {
                var blob = _blobs[i];
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
            var newBlob = new Blob(motherBlob.GetXPos(), motherBlob.GetYPos(), motherBlob.GetRadius(), 8);

            foreach (var blob in _blobs)
            {
                if (blob == null)
                    continue;

                blob.AddBlob(newBlob);
                newBlob.AddBlob(blob);
            }

            if (emptySlot >= _blobs.Count)
                _blobs.Add(newBlob);
            else
                _blobs[emptySlot] = newBlob;

            ++_numActive;
        }

        public int FindSmallest(int exclude)
        {
            var minRadius = 1000.0;
            var minIndex = 0;

            for (var i = 0; i < _blobs.Count; ++i)
            {
                var blob = _blobs[i];
                if (i == exclude || blob == null || blob.GetRadius() >= minRadius)
                    continue;

                minIndex = i;
                minRadius = blob.GetRadius();
            }

            return minIndex;
        }

        public int FindClosest(int exclude)
        {
            var minDist = 1000.0;
            var foundIndex = 0;
            var myPointMass = _blobs[exclude].GetMiddlePointMass();

            for (var i = 0; i < _blobs.Count; ++i)
            {
                var blob = _blobs[i];
                if (i == exclude || blob == null)
                    continue;

                var otherPointMass = blob.GetMiddlePointMass();
                var aXbX = myPointMass.GetXPos() - otherPointMass.GetXPos();
                var aYbY = myPointMass.GetYPos() - otherPointMass.GetYPos();
                var dist = aXbX * aXbX + aYbY * aYbY;
                if (dist >= minDist)
                    continue;

                minDist = dist;
                foundIndex = i;
            }

            return foundIndex;
        }

        public void Join()
        {
            if (_numActive <= 1)
                return;

            var blob1Index = FindSmallest(-1);
            var blob2Index = FindClosest(blob1Index);
            var r1 = _blobs[blob1Index].GetRadius();
            var r2 = _blobs[blob2Index].GetRadius();
            var r3 = Math.Sqrt(r1 * r1 + r2 * r2);
            _blobs[blob1Index] = null;
            _blobs[blob2Index].Scale(0.945 * r3 / r2);
            --_numActive;
        }

        public Point SelectBlob(double x, double y)
        {
            if (_selectedBlob != null)
                return null;

            var minDist = Double.MaxValue;
            Point selectOffset = null;

            foreach (var blob in _blobs)
            {
                if (blob == null)
                    continue;

                var otherPointMass = blob.GetMiddlePointMass();
                var aXbX = x - otherPointMass.GetXPos();
                var aYbY = y - otherPointMass.GetYPos();
                var dist = aXbX * aXbX + aYbY * aYbY;
                if (dist >= minDist)
                    continue;

                minDist = dist;
                if (dist >= blob.GetRadius() / 2.0)
                    continue;

                _selectedBlob = blob;
                selectOffset = new Point(aXbX, aYbY);
            }

            if (_selectedBlob != null)
                _selectedBlob.SetSelected(true);

            return selectOffset;
        }

        public void UnselectBlob()
        {
            if (_selectedBlob == null)
                return;

            _selectedBlob.SetSelected(false);
            _selectedBlob = null;
        }

        public void SelectedBlobMoveTo(double x, double y)
        {
            if (_selectedBlob == null)
                return;

            _selectedBlob.MoveTo(x, y);
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

                var force1 = blob == _selectedBlob
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
                if (blob == null || blob == _selectedBlob)
                    continue;

                var tmpForce = new Vector(0.0, 0.0);
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