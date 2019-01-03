using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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
        private readonly Environment _env = new Environment(0.2, 0.2, 2.6, 1.6);
        private readonly BlobCollective _blobColl = new BlobCollective(1.0, 1.0, 0xC0);
        private readonly DispatcherTimer _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(30) };

        private readonly Vector _upForce = new Vector(0.0, -50.0);
        private readonly Vector _downForce = new Vector(0.0, 50.0);
        private readonly Vector _leftForce = new Vector(-50.0, 0.0);
        private readonly Vector _rightForce = new Vector(50.0, 0.0);

        public MainWindow()
        {
            InitializeComponent();

            _timer.Tick += OnTimerOnTick;
            _timer.Start();

            KeyDown += MainWindow_KeyDown;
        }

        private void OnTimerOnTick(object s, EventArgs e)
        {
            MyCanvas.Children.Clear();

            _blobColl.Move(0.05);
            _blobColl.Sc(_env);
            _blobColl.SetForce(_gravity);
            _blobColl.Draw(MyCanvas, ScaleFactor);
        }

        public void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.G:
                    var y = _gravity.Y > 0.0 ? 0.0 : 10.0;
                    _gravity.Y = y;
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

            }
        }
    }
}