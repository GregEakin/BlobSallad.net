using BlobSallad;
using Xunit;

namespace BlobSalladTests;

public class VectorTests
{
    [Fact]
    public void CtorTest()
    {
        var vector = new Vector(71, 67);
        Assert.Equal(71.0, vector.X);
        Assert.Equal(67.0, vector.Y);
    }

    [Fact]
    public void AddXTest()
    {
        var vector = new Vector(71.0, 67.0);
        vector.AddX(100.0);
        Assert.Equal(171.0, vector.X);
    }

    [Fact]
    public void AddYTest()
    {
        var vector = new Vector(71.0, 67.0);
        vector.AddY(100.0);
        Assert.Equal(167.0, vector.Y);
    }

    [Fact]
    public void SetTest()
    {
        var vector = new Vector(71.0, 67.0);
        var setter = new Vector(61.0, 59.0);
        vector.Set(setter);
        Assert.Equal(61.0, vector.X);
        Assert.Equal(59.0, vector.Y);
    }

    [Fact]
    public void AddTest()
    {
        var vector = new Vector(71.0, 67.0);
        var setter = new Vector(13.0, 11.0);
        vector.Add(setter);
        Assert.Equal(84.0, vector.X);
        Assert.Equal(78.0, vector.Y);
    }

    [Fact]
    public void SubTest()
    {
        var vector = new Vector(71.0, 67.0);
        var setter = new Vector(13.0, 11.0);
        vector.Sub(setter);
        Assert.Equal(58.0, vector.X);
        Assert.Equal(56.0, vector.Y);
    }

    [Fact]
    public void OperatorSubtractTest()
    {
        var vector = new Vector(71.0, 67.0);
        var setter = new Vector(13.0, 11.0);
        var b = vector - setter;
        Assert.Equal(58.0, b.X);
        Assert.Equal(56.0, b.Y);
    }

    [Fact]
    public void DotTest()
    {
        var vector = new Vector(71.0, 67.0);
        var setter = new Vector(13.0, 11.0);
        var dot = vector.DotProd(setter);
        Assert.Equal(1660.0, dot);
    }

    [Fact]
    public void LengthTest()
    {
        var vector = new Vector(71.0, 67.0);
        var length = vector.Length;
        Assert.Equal(97.621, length, 0.01);
    }

    [Fact]
    public void ScaleTest()
    {
        var vector = new Vector(71.0, 67.0);
        vector.Scale(2.0);
        Assert.Equal(142.0, vector.X);
        Assert.Equal(134.0, vector.Y);
    }

    [Fact]
    public void StringTest()
    {
        var vector = new Vector(71.0, 67.0);
        Assert.Equal("(X: 71, Y: 67)", vector.ToString());
    }

    [Fact]
    public void SetXTest()
    {
        var vector = new Vector(71.0, 67.0);
        vector.X = 99.0;
        Assert.Equal(99.0, vector.X);
    }

    [Fact]
    public void SetYTest()
    {
        var vector = new Vector(71.0, 67.0);
        vector.Y = 99.0;
        Assert.Equal(99.0, vector.Y);
    }
}