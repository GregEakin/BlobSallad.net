namespace BlobSallad
{
    public class PointMass
    {
        private readonly Vector cur;
        private readonly Vector prev;
        private readonly Vector force = new Vector(0.0, 0.0);
        private readonly Vector result = new Vector(0.0, 0.0);
        private double mass;
        private double friction = 0.01;

        public PointMass(double cx, double cy, double mass)
        {
            this.cur = new Vector(cx, cy);
            this.prev = new Vector(cx, cy);
            this.mass = mass;
        }

        public double getXPos()
        {
            return this.cur.getX();
        }

        public double getYPos()
        {
            return this.cur.getY();
        }

        public Vector getPos()
        {
            return this.cur;
        }

        public double getXPrevPos()
        {
            return this.prev.getX();
        }

        public double getYPrevPos()
        {
            return this.prev.getY();
        }

        public Vector getPrevPos()
        {
            return this.prev;
        }

        public void addXPos(double dx)
        {
            this.cur.addX(dx);
        }

        public void addYPos(double dy)
        {
            this.cur.addY(dy);
        }

        public void setForce(Vector force)
        {
            this.force.set(force);
        }

        public void addForce(Vector force)
        {
            this.force.add(force);
        }

        public double getMass()
        {
            return this.mass;
        }

        public void setMass(double mass)
        {
            this.mass = mass;
        }

        public void move(double dt)
        {
            double dtdt = dt * dt;

            double ax = this.force.getX() / this.mass;
            double cx = this.cur.getX();
            double px = this.prev.getX();
            double tx = (2.0 - this.friction) * cx - (1.0 - this.friction) * px + ax * dtdt;
            this.prev.setX(cx);
            this.cur.setX(tx);

            double ay = this.force.getY() / this.mass;
            double cy = this.cur.getY();
            double py = this.prev.getY();
            double ty = (2.0 - this.friction) * cy - (1.0 - this.friction) * py + ay * dtdt;
            this.prev.setY(cy);
            this.cur.setY(ty);
        }

        public double getVelocity()
        {
            double cXpX = this.cur.getX() - this.prev.getX();
            double cYpY = this.cur.getY() - this.prev.getY();
            return cXpX * cXpX + cYpY * cYpY;
        }

        public void setFriction(double friction)
        {
            this.friction = friction;
        }

        //public void draw(Graphics graphics, double scaleFactor)
        //{
        //    BasicStroke stroke = new BasicStroke(2.0F);
        //    Graphics2D g2d = (Graphics2D)graphics;
        //    g2d.setColor(Color.BLACK);
        //    g2d.setStroke(stroke);
        //    Double arc = new Double();
        //    readonly double x = this.cur.getX() * scaleFactor;
        //    readonly double y = this.cur.getY() * scaleFactor;
        //    readonly double radius = 4.0; // * mass;
        //    arc.setArcByCenter(x, y, radius, 0.0, 360.0, 1);
        //    g2d.fill(arc);
        //}
    }
}