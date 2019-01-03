﻿using System;
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

            var blob = new Blob(100.0, 100.0, 25.0, 5);
            blob.DrawOohFace(MyCanvas, 10.0, transformGroup);
        }
    }
}