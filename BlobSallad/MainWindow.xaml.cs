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

            var translateTransform = new TranslateTransform(100.0, 100.0);
            var blob = new Blob(100.0, 100.0, 25.0, 5);
            blob.DrawOohFace(MyCanvas, 10.0, translateTransform);
        }
    }
}