namespace BlobSallad
{
    public class Joint
    {
        private readonly PointMass pointMassA;
        private readonly PointMass pointMassB;
        private readonly Vector pointMassAPos;
        private readonly Vector pointMassBPos;
        private double shortConst;
        private double longConst;
        private double scSquared;
        private double lcSquared;

        public Joint(PointMass pointMassA, PointMass pointMassB, double shortConst, double longConst)
        {
            this.pointMassA = pointMassA;
            this.pointMassB = pointMassB;
            this.pointMassAPos = pointMassA.getPos();
            this.pointMassBPos = pointMassB.getPos();

            Vector delta = new Vector(this.pointMassBPos);
            delta.sub(this.pointMassAPos);
            double length = delta.length();
            this.shortConst = length * shortConst;
            this.longConst = length * longConst;
            this.scSquared = this.shortConst * this.shortConst;
            this.lcSquared = this.longConst * this.longConst;
        }

        public PointMass getPointMassA()
        {
            return pointMassA;
        }

        public PointMass getPointMassB()
        {
            return pointMassB;
        }

        public double getLongConst()
        {
            return longConst;
        }

        public double getShortConst()
        {
            return shortConst;
        }

        public void setDist(double shortConst, double longConst)
        {
            this.shortConst = shortConst;
            this.longConst = longConst;
            this.scSquared = this.shortConst * this.shortConst;
            this.lcSquared = this.longConst * this.longConst;
        }

        public void scale(double scaleFactor)
        {
            this.shortConst *= scaleFactor;
            this.longConst *= scaleFactor;
            this.scSquared = this.shortConst * this.shortConst;
            this.lcSquared = this.longConst * this.longConst;
        }

        public void sc()
        {
            Vector delta = new Vector(this.pointMassBPos);
            delta.sub(this.pointMassAPos);
            double dp = delta.dotProd(delta);
            if (this.shortConst != 0.0 && dp < this.scSquared)
            {
                double scaleFactor = this.scSquared / (dp + this.scSquared) - 0.5;
                delta.scale(scaleFactor);
                this.pointMassAPos.sub(delta);
                this.pointMassBPos.add(delta);
            }
            else if (this.longConst != 0.0 && dp > this.lcSquared)
            {
                double scaleFactor = this.lcSquared / (dp + this.lcSquared) - 0.5;
                delta.scale(scaleFactor);
                this.pointMassAPos.sub(delta);
                this.pointMassBPos.add(delta);
            }
        }
    }
}