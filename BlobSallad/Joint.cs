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
            this._pointMassA = pointMassA;
            this._pointMassB = pointMassB;
            this._pointMassAPos = pointMassA.GetPos();
            this._pointMassBPos = pointMassB.GetPos();

            Vector delta = new Vector(this._pointMassBPos);
            delta.Sub(this._pointMassAPos);
            double length = delta.Length();
            this._shortConst = length * shortConst;
            this._longConst = length * longConst;
            this._scSquared = this._shortConst * this._shortConst;
            this._lcSquared = this._longConst * this._longConst;
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
            this._shortConst = shortConst;
            this._longConst = longConst;
            this._scSquared = this._shortConst * this._shortConst;
            this._lcSquared = this._longConst * this._longConst;
        }

        public void Scale(double scaleFactor)
        {
            this._shortConst *= scaleFactor;
            this._longConst *= scaleFactor;
            this._scSquared = this._shortConst * this._shortConst;
            this._lcSquared = this._longConst * this._longConst;
        }

        public void Sc()
        {
            Vector delta = new Vector(this._pointMassBPos);
            delta.Sub(this._pointMassAPos);
            double dp = delta.DotProd(delta);
            if (this._shortConst != 0.0 && dp < this._scSquared)
            {
                double scaleFactor = this._scSquared / (dp + this._scSquared) - 0.5;
                delta.Scale(scaleFactor);
                this._pointMassAPos.Sub(delta);
                this._pointMassBPos.Add(delta);
            }
            else if (this._longConst != 0.0 && dp > this._lcSquared)
            {
                double scaleFactor = this._lcSquared / (dp + this._lcSquared) - 0.5;
                delta.Scale(scaleFactor);
                this._pointMassAPos.Sub(delta);
                this._pointMassBPos.Add(delta);
            }
        }
    }
}