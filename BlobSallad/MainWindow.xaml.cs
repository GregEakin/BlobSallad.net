using System.Windows;
using System.Windows.Controls;

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

            var blob = new Blob(100.0, 100.0, 25.0, 5);
            blob.drawOohFace(MyCanvas, 10.0);
        }
    }
}