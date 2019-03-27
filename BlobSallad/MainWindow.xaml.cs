using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace BlobSallad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double ScaleFactor = 200.0;
        private readonly Vector _gravity = new Vector(0.0, 10.0);
        private readonly Environment _env = new Environment(0.2, 0.2, 3.6, 1.85);
        private readonly BlobCollective _blobColl = new BlobCollective(1.0, 1.0, 0xC0);
        private readonly DispatcherTimer _timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(30)};

        private readonly Vector _upForce = new Vector(0.0, -50.0);
        private readonly Vector _downForce = new Vector(0.0, 50.0);
        private readonly Vector _leftForce = new Vector(-50.0, 0.0);
        private readonly Vector _rightForce = new Vector(50.0, 0.0);

        private Point? _savedMouseCoords;
        private Point? _selectOffset;
        private bool _stopped;

        public MainWindow()
        {
            InitializeComponent();

            _timer.Tick += OnTimerOnTick;
            _timer.Start();

            KeyDown += MainWindow_KeyDown;

            MouseDown += MainWindow_MouseDown;
            MouseUp += MainWindow_MouseUp;
            MouseMove += MainWindow_MouseMove;
            MouseLeave += MainWindow_MouseLeave;

            SizeChanged += MainWindow_SizeChanged;
        }

        private void ToggleGravity()
        {
            var y = _gravity.Y > 0.0 ? 0.0 : 10.0;
            _gravity.Y = y;
        }

        public void Stop()
        {
            _stopped = true;
            _timer.Stop();
        }

        public void Start()
        {
            _stopped = false;
            _timer.Start();
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

        public void Draw()
        {
            MyCanvas.Children.Clear();
            _env.Draw(MyCanvas, ScaleFactor);
            _blobColl.Draw(MyCanvas, ScaleFactor);
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var w = (Width - 80.0) / ScaleFactor;
            _env.Width = w;

            var h = (Height - 80.0) / ScaleFactor;
            _env.Height = h;
        }

        private void OnTimerOnTick(object s, EventArgs e)
        {
            Update();
            Draw();
        }

        public void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.G:
                    ToggleGravity();
                    break;

                case Key.H:
                    _blobColl.Split();
                    break;

                case Key.J:
                    _blobColl.Join();
                    break;

                case Key.Up:
                    _blobColl.AddForce(_upForce);
                    break;

                case Key.Down:
                    _blobColl.AddForce(_downForce);
                    break;

                case Key.Left:
                    _blobColl.AddForce(_leftForce);
                    break;

                case Key.Right:
                    _blobColl.AddForce(_rightForce);
                    break;

                case Key.S:
                    Stop();
                    break;

                case Key.Q:
                    Start();
                    break;

                case Key.Escape:
                    Application.Current.Shutdown();
                    break;
            }
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            if (_stopped)
                return;

            if (_selectOffset == null)
                return;

            var mouseCoords = GetMouseCoords(e);

            var x = mouseCoords.X - _selectOffset.Value.X;
            var y = mouseCoords.Y - _selectOffset.Value.Y;
            _blobColl.SelectedBlobMoveTo(x, y);
            _savedMouseCoords = mouseCoords;
        }

        private void MainWindow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            if (_stopped)
                return;

            var mouseCoords = GetMouseCoords(e);
            var x = mouseCoords.X;
            var y = mouseCoords.Y;
            _selectOffset = _blobColl.FindClosest(x, y);
        }

        private void MainWindow_MouseUp(object sender, MouseEventArgs e)
        {
            _blobColl.UnselectBlob();
            _savedMouseCoords = null;
            _selectOffset = null;
        }

        private void MainWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            _blobColl.UnselectBlob();
            _savedMouseCoords = null;
            _selectOffset = null;
        }

        public Point GetMouseCoords(MouseEventArgs e)
        {
            var x = e.GetPosition(this).X / ScaleFactor;
            var y = e.GetPosition(this).Y / ScaleFactor;
            return new Point(x, y);
        }
    }
}