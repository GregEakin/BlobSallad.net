using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace BlobSallad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var transformGroup = new TransformGroup();
            var translateTransform = new TranslateTransform(100.0, 100.0);
            transformGroup.Children.Add(translateTransform);

            //var blob = new Blob(100.0, 100.0, 25.0, 5);
            //blob.DrawOohFace(MyCanvas, 10.0, transformGroup);

            var gravity = new Vector(0.0, 10.0);

            var env = new Environment(0.2, 0.2, 2.6, 1.6);

            var blobColl = new BlobCollective(1.0, 1.0, 0xC0);

            for (var i = 0; i < 100; i++)
            {
                blobColl.Move(0.05);
                blobColl.Sc(env);
                blobColl.SetForce(gravity);
                blobColl.Draw(MyCanvas, 200.0);
                // Thread.Sleep(50);

                if (i == 14)
                    blobColl.AddForce(new Vector(500, -10.0));
            }
        }
    }
}