using System.Windows.Controls;

namespace BlobSallad
{
    public class Controller
    {
        private readonly double scaleFactor = 200.0;
        private readonly BlobCollective blobColl = new BlobCollective(1.0, 1.0, 0xC0);
        private readonly Vector gravity = new Vector(0.0, 10.0);
        private volatile bool stopped = false;
        private Environment env = new Environment(0.2, 0.2, 2.6, 1.6);
        private Point savedMouseCoords = null;

        private Point selectOffset = null;
        // private Timer timer;

        private void toggleGravity()
        {
            double y = this.gravity.getY() > 0.0 ? 0.0 : 10.0;
            this.gravity.setY(y);
        }

        public void stop()
        {
            this.stopped = true;
        }

        public void start()
        {
            this.stopped = false;
            // this.timeout();
        }

        public void update()
        {
            if (this.savedMouseCoords != null && this.selectOffset != null)
            {
                double x = this.savedMouseCoords.getX() - this.selectOffset.getX();
                double y = this.savedMouseCoords.getY() - this.selectOffset.getY();
                this.blobColl.selectedBlobMoveTo(x, y);
            }

            this.blobColl.move(0.05);
            this.blobColl.sc(this.env);
            this.blobColl.setForce(this.gravity);
        }

        public void paintComponent(Canvas canvas)
        {
            // int width = this.view.getWidth();
            // readonly double w =
            // ((double) width - 80.0) / 200.0;
            // int height = this.view.getHeight();
            // readonly double h =
            // ((double) height - 80.0) / 200.0;
            // this.env = this.env.setWidth(w).setHeight(h);
            // graphics.clearRect(0, 0, width, height);
            // Graphics2D g2d = (Graphics2D) graphics.create();
            // g2d.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
            // this.env.draw(g2d, 200.0);
            // this.blobColl.draw(g2d, 200.0);
            // g2d.dispose();
        }

        public void draw()
        {
            // this.view.repaint();
        }

        private void timeout()
        {
            // if (this.timer != null)
            //     this.timer.cancel();

            // this.timer = new Timer();
            // TimerTask task = new TimerTask()
            // {

            //     public void run()
            //     {
            //     Controller.this.draw();
            //     Controller.this.update();
            //     if (!Controller.this.stopped)
            //     return;
            //     this.cancel();
            // }
            // };
            // this.timer.schedule(task, 0L, 30L);
        }

        public Controller()
        {
            // this.view.setDoubleBuffered(true);

            // readonly KeyListener keyListener =
            // new KeyListener()
            // {

            //     private readonly Vector upForce = new Vector(0.0, -50.0);
            //     private readonly Vector downForce = new Vector(0.0, 50.0);
            //     private readonly Vector leftForce = new Vector(-50.0, 0.0);
            //     private readonly Vector rightForce = new Vector(50.0, 0.0);

            //     public void keyTyped(KeyEvent e)
            //     {
            //     }

            //     public void keyPressed(KeyEvent e)
            //     {
            //         int code = e.getKeyCode();
            //         switch (code)
            //         {
            //             case VK_G:
            //                 Controller.this.toggleGravity();
            //                 break;
            //             case VK_H:
            //                 Controller.this.blobColl.split();
            //                 break;
            //             case VK_J:
            //                 Controller.this.blobColl.join();
            //                 break;
            //             case VK_UP:
            //                 Controller.this.blobColl.addForce(upForce);
            //                 break;
            //             case VK_DOWN:
            //                 Controller.this.blobColl.addForce(downForce);
            //                 break;
            //             case VK_LEFT:
            //                 Controller.this.blobColl.addForce(leftForce);
            //                 break;
            //             case VK_RIGHT:
            //                 Controller.this.blobColl.addForce(rightForce);
            //                 break;
            //         }
            //     }

            //     public void keyReleased(KeyEvent e)
            //     {
            //     }
            // };
            // this.view.addKeyListener(keyListener);

            // readonly MouseMotionListener motionListener = new MouseMotionListener()
            // {
            //     public void mouseDragged(MouseEvent e)
            //     {
            //         if (Controller.this.stopped)
            //         return;
                   
            //         if (Controller.this.selectOffset == null)
            //         return;
                   
            //         Point mouseCoords = Controller.this.getMouseCoords(e);
            //         if (mouseCoords == null)
            //             return;
                   
            //         readonly double x =
            //         mouseCoords.getX() - Controller.this.selectOffset.getX();
            //         readonly double y =
            //         mouseCoords.getY() - Controller.this.selectOffset.getY();
            //         Controller.this.blobColl.selectedBlobMoveTo(x, y);
            //         Controller.this.savedMouseCoords = mouseCoords;
            //     }
                   
            //     public void mouseMoved(MouseEvent e)
            //     {
            //     }
            // };
            // this.view.addMouseMotionListener(motionListener);

            // readonly MouseListener mouseListener = new MouseListener()
            // {
            //     public void mouseClicked(MouseEvent e)
            //     {
            //         Controller.this.requestFocus();
            //     }
                   
            //     public void mousePressed(MouseEvent e)
            //     {
            //         if (Controller.this.stopped)
            //         return;
                   
            //         Point mouseCoords = Controller.this.getMouseCoords(e);
            //         if (mouseCoords == null)
            //             return;
                   
            //         readonly double x =
            //         mouseCoords.getX();
            //         readonly double y =
            //         mouseCoords.getY();
            //         Controller.this.selectOffset = Controller.this.blobColl.selectBlob(x, y);
            //     }
                   
            //     public void mouseReleased(MouseEvent e)
            //     {
            //         Controller.this.blobColl.unselectBlob();
            //         Controller.this.savedMouseCoords = null;
            //         Controller.this.selectOffset = null;
            //     }
                   
            //     public void mouseEntered(MouseEvent e)
            //     {
            //     }
                   
            //     public void mouseExited(MouseEvent e)
            //     {
            //     }
            // };
            // this.view.addMouseListener(mouseListener);
            // this.requestFocus();
        }

        //public Point getMouseCoords(MouseEvent e)
        //{
        //    readonly double x = (double)e.getX() / 200.0;
        //    readonly double y = (double)e.getY() / 200.0;
        //    return new Point(x, y);
        //}

        public void requestFocus()
        {
            //this.view.setFocusable(true);
            //this.view.requestFocus();
            //this.view.requestFocusInWindow();
        }

        //public View getView()
        //{
        //    //return this.view;
        //}
    }
}