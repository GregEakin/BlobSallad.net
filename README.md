# Blob Sallad
I found this little blob engine code, and converted it run in WPF under C#.
The GUI drawing code is verified with [ApprovalTests.Net](https://github.com/approvals/ApprovalTests.Net).

## From the website:
[![Blob Sallad](Blob.PNG)](https://blobsallad.se/)

Original version by: [Bjoern Lindberg](mailto:bjoern.lindberg@gmail.com)

## Sample code
Here's a test that verifies the Ooh Face [OohFace](BlobSalladTests/BlobTests.DrawOohFaceTest.Microsoft_Windows_10_Pro.approved.png)
Creates a canvas, identifies the translate tranformation, executes the test code, and verifies the results.
```C#
[Test]
public void DrawOohFaceTest()
{
    var canvas = new Canvas {Width = 100, Height = 100};

    var translateTransform = new TranslateTransform(50.0, 50.0);
    var transformGroup = new TransformGroup();
    transformGroup.Children.Add(translateTransform);

    var blob = new Blob(50.0, 50.0, 25.0, 5);
    blob.DrawOohFace(canvas, 3.0, transformGroup);

    var wpf = new ContentControl {Content = canvas};
    WpfApprovals.Verify(wpf);
}

```

## Links:
- [Community Edition of Visual Studio (Free)](https://www.visualstudio.com/vs/community/)
- [Approval Tests](http://approvaltests.com/)
- [Git Extensions (Free)](http://gitextensions.github.io/)
- [ReSharper, Extensions for .NET Developers](https://www.jetbrains.com/resharper/)

## Author
:fire: [Greg Eakin](https://www.linkedin.com/in/gregeakin)
