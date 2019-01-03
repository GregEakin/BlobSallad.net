// Found from https://blobsallad.se/
// Originally Written by: bjoern.lindberg@gmail.com
// Translated to C# by Greg Eakin

using System.Windows;
using System.Windows.Controls;

namespace BlobSallad
{
    public class Controller
    {
        private readonly double _scaleFactor = 200.0;
        private readonly BlobCollective _blobColl = new BlobCollective(1.0, 1.0, 0xC0);
        private readonly Vector _gravity = new Vector(0.0, 10.0);
        private volatile bool _stopped = false;
        private Environment _env = new Environment(0.2, 0.2, 2.6, 1.6);
        private Point? _savedMouseCoords = null;
        private Point? _selectOffset = null;
        // private Timer timer;

        private void ToggleGravity()
        {
            var y = _gravity.Y > 0.0 ? 0.0 : 10.0;
            _gravity.Y = y;
        }

        public void Stop()
        {
            _stopped = true;
        }

        public void Start()
        {
            _stopped = false;
            // this.timeout();
        }

        public void Update()
        {
            if (_savedMouseCoords != null && _selectOffset != null)
            {
                var x = _savedMouseCoords.Value.X - _selectOffset.Value.X;
                var y = _savedMouseCoords.Value.Y - _selectOffset.Value.Y;
                _blobColl.SelectedBlobMoveTo(x, y);
            }

            _blobColl.Move(0.05);
            _blobColl.Sc(_env);
            _blobColl.Force = _gravity;
        }

        public void PaintComponent(Canvas canvas)
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

        public void Draw()
        {
            // this.view.repaint();
        }

        private void Timeout()
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

        public void RequestFocus()
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