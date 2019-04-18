// Found from https://blobsallad.se/
// Originally Written by: bjoern.lindberg@gmail.com
// Translated to C# by Greg Eakin

using System;
using System.Windows.Controls;

namespace BlobSallad
{
    public class Bone : Connection
    {
        private double _slSquared;
        private double _llSquared;

        public Bone(PointMass pointMassA, PointMass pointMassB, double shortFactor, double longFactor)
            : base(pointMassA, pointMassB)
        {
            if (shortFactor < 0)
                throw new Exception("Short Factor needs to be greater than zero.");
            if (longFactor < 0)
                throw new Exception("Long Factor needs to be greater than zero.");
            if (shortFactor >= longFactor)
                throw new Exception("Short Factor needs to be less than Long Factor.");

            var delta = PointMassB.Pos - PointMassA.Pos;
            ShortLimit = delta.Length * shortFactor;
            LongLimit = delta.Length * longFactor;
            _slSquared = ShortLimit * ShortLimit;
            _llSquared = LongLimit * LongLimit;
        }

        public double LongLimit { get; private set; }

        public double ShortLimit { get; private set; }

        public override void Scale(double scaleFactor)
        {
            ShortLimit *= scaleFactor;
            LongLimit *= scaleFactor;
            _slSquared = ShortLimit * ShortLimit;
            _llSquared = LongLimit * LongLimit;
        }

        public override void Sc(Environment env)
        {
            var delta = PointMassB.Pos - PointMassA.Pos;
            var distance = delta.DotProd(delta);
            if (distance < _slSquared)
            {
                var scaleFactor = _slSquared / (distance + _slSquared) - 0.5;
                delta.Scale(scaleFactor);
                PointMassA.Pos.Sub(delta);
                PointMassB.Pos.Add(delta);
            }
            else if (distance > _llSquared)
            {
                var scaleFactor = _llSquared / (distance + _llSquared) - 0.5;
                delta.Scale(scaleFactor);
                PointMassA.Pos.Sub(delta);
                PointMassB.Pos.Add(delta);
            }
        }

        public override void Draw(Canvas canvas, double scaleFactor)
        {
        }
    }
}