// Found from https://blobsallad.se/
// Originally Written by: bjoern.lindberg@gmail.com
// Translated to C# by Greg Eakin

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BlobSallad
{
    public class BlobCollective
    {
        private const int BlobPointMasses = 8;
        private const double BlobInitialRadius = 0.4;

        private readonly Random _random = new Random();

        private readonly List<Blob> _blobs = new List<Blob>();

        public BlobCollective(double x, double y, int maxNum)
        {
            MaxNum = maxNum;
            _blobs.Add(new Blob(x, y, BlobInitialRadius, BlobPointMasses));
        }

        public int MaxNum { get; }

        public int NumActive { get; private set; } = 1;

        public Blob SelectedBlob { get; private set; }

        public void Split()
        {
            var maxRadius = 0.0;
            Blob motherBlob = null;
            if (NumActive == MaxNum)
                return;

            var emptySlot = _blobs.Count;

            for (var i = 0; i < _blobs.Count; ++i)
            {
                var blob = _blobs[i];
                if (blob == null)
                {
                    emptySlot = i;
                }
                else if (blob.Radius > maxRadius)
                {
                    maxRadius = blob.Radius;
                    motherBlob = blob;
                }
            }

            if (motherBlob == null)
                return;

            motherBlob.Scale(0.75);
            var newBlob = new Blob(motherBlob.XMiddle, motherBlob.YMiddle, motherBlob.Radius, BlobPointMasses);

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

            ++NumActive;
        }

        public int FindSmallest(int exclude)
        {
            var minRadius = 1000.0;
            var minIndex = 0;

            for (var i = 0; i < _blobs.Count; ++i)
            {
                var blob = _blobs[i];
                if (i == exclude || blob == null || blob.Radius >= minRadius)
                    continue;

                minIndex = i;
                minRadius = blob.Radius;
            }

            return minIndex;
        }

        public int FindClosest(int exclude)
        {
            var minDist = 1000.0;
            var foundIndex = 0;
            var myPointMass = _blobs[exclude].MiddlePointMass;

            for (var i = 0; i < _blobs.Count; ++i)
            {
                var blob = _blobs[i];
                if (i == exclude || blob == null)
                    continue;

                var otherPointMass = blob.MiddlePointMass;
                var aXbX = myPointMass.XPos - otherPointMass.XPos;
                var aYbY = myPointMass.YPos - otherPointMass.YPos;
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
            if (NumActive <= 1)
                return;

            var smallest = FindSmallest(-1);
            var blobSmallest = _blobs[smallest];
            var closest = FindClosest(smallest);
            var blobClosest = _blobs[closest];
            var r1 = blobSmallest.Radius;
            var r2 = blobClosest.Radius;
            var r3 = Math.Sqrt(r1 * r1 + r2 * r2);
            blobClosest.Scale(0.945 * r3 / r2);

            foreach (var blob in _blobs)
            {
                if (blob == blobSmallest)
                    continue;

                blob?.RemoveBlob(blobSmallest);
            }


            blobSmallest.Dispose();
            _blobs[smallest] = null;
            --NumActive;
        }

        public Point? SelectBlob(double x, double y)
        {
            if (SelectedBlob != null)
                return null;

            var minDist = double.MaxValue;
            Point? selectOffset = null;

            foreach (var blob in _blobs)
            {
                if (blob == null)
                    continue;

                var otherPointMass = blob.MiddlePointMass;
                var aXbX = x - otherPointMass.XPos;
                var aYbY = y - otherPointMass.YPos;
                var dist = aXbX * aXbX + aYbY * aYbY;
                if (dist >= minDist)
                    continue;

                minDist = dist;
                if (dist >= blob.Radius / 2.0)
                    continue;

                SelectedBlob = blob;
                selectOffset = new Point(aXbX, aYbY);
            }

            if (SelectedBlob != null)
                SelectedBlob.Selected = true;

            return selectOffset;
        }

        public void UnselectBlob()
        {
            if (SelectedBlob == null)
                return;

            SelectedBlob.Selected = false;
            SelectedBlob = null;
        }

        public void SelectedBlobMoveTo(double x, double y)
        {
            SelectedBlob?.MoveTo(x, y);
        }

        public void Move(double dt)
        {
            foreach (var blob in _blobs)
                blob?.Move(dt);
        }

        public void Sc(Environment env)
        {
            foreach (var blob in _blobs)
                blob?.Sc(env);
        }

        public Vector Force
        {
            set
            {
                foreach (var blob in _blobs)
                {
                    if (blob == null)
                        continue;

                    var force = blob == SelectedBlob
                        ? new Vector(0.0, 0.0)
                        : value;
                    blob.Force = force;
                }
            }
        }

        public void AddForce(Vector force)
        {
            foreach (var blob in _blobs)
            {
                if (blob == null || blob == SelectedBlob)
                    continue;

                var x = force.X * (_random.NextDouble() * 0.75 + 0.25);
                var y = force.Y * (_random.NextDouble() * 0.75 + 0.25);
                var newForce = new Vector(x, y);
                blob.AddForce(newForce);
            }
        }

        public void Draw(Canvas canvas, double scaleFactor)
        {
            foreach (var blob in _blobs)
            {
                blob?.Draw(canvas, scaleFactor);
            }
        }
    }
}