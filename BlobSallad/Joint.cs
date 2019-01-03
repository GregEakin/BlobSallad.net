namespace BlobSallad
{
    public class Joint
    {
        private readonly Vector _pointMassAPos;
        private readonly Vector _pointMassBPos;
        private double _scSquared;
        private double _lcSquared;

        public Joint(PointMass pointMassA, PointMass pointMassB, double shortConst, double longConst)
        {
            PointMassA = pointMassA;
            PointMassB = pointMassB;
            _pointMassAPos = pointMassA.Pos;
            _pointMassBPos = pointMassB.Pos;

            var delta = new Vector(_pointMassBPos);
            delta.Sub(_pointMassAPos);
            var length = delta.Length();
            ShortConst = length * shortConst;
            LongConst = length * longConst;
            _scSquared = ShortConst * ShortConst;
            _lcSquared = LongConst * LongConst;
        }

        public PointMass PointMassA { get; }

        public PointMass PointMassB { get; }

        public double LongConst { get; private set; }

        public double ShortConst { get; private set; }

        public void SetDist(double shortConst, double longConst)
        {
            ShortConst = shortConst;
            LongConst = longConst;
            _scSquared = ShortConst * ShortConst;
            _lcSquared = LongConst * LongConst;
        }

        public void Scale(double scaleFactor)
        {
            ShortConst *= scaleFactor;
            LongConst *= scaleFactor;
            _scSquared = ShortConst * ShortConst;
            _lcSquared = LongConst * LongConst;
        }

        public void Sc()
        {
            var delta = new Vector(_pointMassBPos);
            delta.Sub(_pointMassAPos);
            var dp = delta.DotProd(delta);
            if (ShortConst != 0.0 && dp < _scSquared)
            {
                var scaleFactor = _scSquared / (dp + _scSquared) - 0.5;
                delta.Scale(scaleFactor);
                _pointMassAPos.Sub(delta);
                _pointMassBPos.Add(delta);
            }
            else if (LongConst != 0.0 && dp > _lcSquared)
            {
                var scaleFactor = _lcSquared / (dp + _lcSquared) - 0.5;
                delta.Scale(scaleFactor);
                _pointMassAPos.Sub(delta);
                _pointMassBPos.Add(delta);
            }
        }
    }
}