// Found from https://blobsallad.se/
// Originally Written by: bjoern.lindberg@gmail.com
// Translated to C# by Greg Eakin

namespace BlobSallad
{
    public class Joint
    {
        private double _slSquared;
        private double _llSquared;

        public Joint(PointMass pointMassA, PointMass pointMassB, double shortLimit, double longLimit)
        {
            PointMassA = pointMassA;
            PointMassB = pointMassB;

            var delta = PointMassB.Pos - PointMassA.Pos;
            ShortLimit = delta.Length * shortLimit;
            LongLimit = delta.Length * longLimit;
            _slSquared = ShortLimit * ShortLimit;
            _llSquared = LongLimit * LongLimit;
        }

        public PointMass PointMassA { get; }

        public PointMass PointMassB { get; }

        public double LongLimit { get; private set; }

        public double ShortLimit { get; private set; }

        public void SetLimit(double shortLimit, double longLimit)
        {
            ShortLimit = shortLimit;
            LongLimit = longLimit;
            _slSquared = ShortLimit * ShortLimit;
            _llSquared = LongLimit * LongLimit;
        }

        public void Scale(double scaleFactor)
        {
            ShortLimit *= scaleFactor;
            LongLimit *= scaleFactor;
            _slSquared = ShortLimit * ShortLimit;
            _llSquared = LongLimit * LongLimit;
        }

        public void Sc()
        {
            var delta = PointMassB.Pos - PointMassA.Pos;
            var dp = delta.DotProd(delta);
            if (dp < _slSquared)
            {
                var scaleFactor = _slSquared / (dp + _slSquared) - 0.5;
                delta.Scale(scaleFactor);
                PointMassA.Pos.Sub(delta);
                PointMassB.Pos.Add(delta);
            }
            else if (dp > _llSquared)
            {
                var scaleFactor = _llSquared / (dp + _llSquared) - 0.5;
                delta.Scale(scaleFactor);
                PointMassA.Pos.Sub(delta);
                PointMassB.Pos.Add(delta);
            }
        }
    }
}