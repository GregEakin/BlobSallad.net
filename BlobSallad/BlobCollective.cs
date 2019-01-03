﻿// Found from https://blobsallad.se/
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
            if (NumActive >= MaxNum)
                return;

            var motherBlob = FindLargest(null);
            if (motherBlob == null)
                return;

            motherBlob.Scale(0.75);
            var newBlob = new Blob(motherBlob.XMiddle, motherBlob.YMiddle, motherBlob.Radius, BlobPointMasses);

            foreach (var blob in _blobs)
            {
                blob.LinkBlob(newBlob);
                newBlob.LinkBlob(blob);
            }

            _blobs.Add(newBlob);

            ++NumActive;
        }

        public void Join()
        {
            if (NumActive <= 1)
                return;

            var smallest = FindSmallest(null);
            var closest = FindClosest(smallest);
            var distance = Math.Sqrt(smallest.Radius * smallest.Radius + closest.Radius * closest.Radius);
            closest.Scale(0.945 * distance / closest.Radius);

            foreach (var blob in _blobs)
            {
                if (blob == smallest)
                    continue;

                blob.UnLinkBlob(smallest);
            }

            _blobs.Remove(smallest);
            smallest.Dispose();
            --NumActive;
        }

        public Blob FindLargest(Blob exclude)
        {
            var maxRadius = double.MinValue;
            Blob motherBlob = null;

            foreach (var blob in _blobs)
            {
                if (blob == exclude)
                    continue;

                if (blob.Radius <= maxRadius)
                    continue;

                maxRadius = blob.Radius;
                motherBlob = blob;
            }

            return motherBlob;
        }

        public Blob FindSmallest(Blob exclude)
        {
            var minRadius = double.MaxValue;
            Blob smallest = null;

            foreach (var blob in _blobs)
            {
                if (blob == exclude)
                    continue;

                if (blob.Radius >= minRadius)
                    continue;

                minRadius = blob.Radius;
                smallest = blob;
            }

            return smallest;
        }

        public Blob FindClosest(Blob exclude)
        {
            var excludeMiddlePointMass = exclude.MiddlePointMass;
            var minDist = double.MaxValue;
            Blob findClosest = null;
            foreach (var blob in _blobs)
            {
                if (blob == exclude)
                    continue;

                var blobMiddlePointMass = blob.MiddlePointMass;
                var aXbX = excludeMiddlePointMass.XPos - blobMiddlePointMass.XPos;
                var aYbY = excludeMiddlePointMass.YPos - blobMiddlePointMass.YPos;
                var dist = aXbX * aXbX + aYbY * aYbY;
                if (dist >= minDist)
                    continue;

                minDist = dist;
                findClosest = blob;
            }

            return findClosest;
        }

        public Point? SelectBlob(double x, double y)
        {
            if (SelectedBlob != null)
                return null;

            var minDist = double.MaxValue;
            Point? selectOffset = null;

            foreach (var blob in _blobs)
            {
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
            SelectedBlob.MoveTo(x, y);
        }

        public void Move(double dt)
        {
            foreach (var blob in _blobs)
                blob.Move(dt);
        }

        public void Sc(Environment env)
        {
            foreach (var blob in _blobs)
                blob.Sc(env);
        }

        public Vector Force
        {
            set
            {
                foreach (var blob in _blobs)
                {
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
                if (blob == SelectedBlob)
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
                blob.Draw(canvas, scaleFactor);
        }
    }
}