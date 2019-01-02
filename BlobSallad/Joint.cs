namespace BlobSallad
{
    public class Joint
    {
        private readonly PointMass _pointMassA;
        private readonly PointMass _pointMassB;
        private readonly Vector _pointMassAPos;
        private readonly Vector _pointMassBPos;
        private double _shortConst;
        private double _longConst;
        private double _scSquared;
        private double _lcSquared;

        public Joint(PointMass pointMassA, PointMass pointMassB, double shortConst, double longConst)
        {
            _pointMassA = pointMassA;
            _pointMassB = pointMassB;
            _pointMassAPos = pointMassA.GetPos();
            _pointMassBPos = pointMassB.GetPos();

            var delta = new Vector(_pointMassBPos);
            delta.Sub(_pointMassAPos);
            var length = delta.Length();
            _shortConst = length * shortConst;
            _longConst = length * longConst;
            _scSquared = _shortConst * _shortConst;
            _lcSquared = _longConst * _longConst;
        }

        public PointMass GetPointMassA()
        {
            return _pointMassA;
        }

        public PointMass GetPointMassB()
        {
            return _pointMassB;
        }

        public double GetLongConst()
        {
            return _longConst;
        }

        public double GetShortConst()
        {
            return _shortConst;
        }

        public void SetDist(double shortConst, double longConst)
        {
            _shortConst = shortConst;
            _longConst = longConst;
            _scSquared = _shortConst * _shortConst;
            _lcSquared = _longConst * _longConst;
        }

        public void Scale(double scaleFactor)
        {
            _shortConst *= scaleFactor;
            _longConst *= scaleFactor;
            _scSquared = _shortConst * _shortConst;
            _lcSquared = _longConst * _longConst;
        }

        public void Sc()
        {
            var delta = new Vector(_pointMassBPos);
            delta.Sub(_pointMassAPos);
            var dp = delta.DotProd(delta);
            if (_shortConst != 0.0 && dp < _scSquared)
            {
                var scaleFactor = _scSquared / (dp + _scSquared) - 0.5;
                delta.Scale(scaleFactor);
                _pointMassAPos.Sub(delta);
                _pointMassBPos.Add(delta);
            }
            else if (_longConst != 0.0 && dp > _lcSquared)
            {
                var scaleFactor = _lcSquared / (dp + _lcSquared) - 0.5;
                delta.Scale(scaleFactor);
                _pointMassAPos.Sub(delta);
                _pointMassBPos.Add(delta);
            }
        }
    }
}