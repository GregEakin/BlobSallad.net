using BlobSallad;
using Xunit;

namespace BlobSalladTests;

public class EnvironmentTests
{
    [Fact]
    public void CtorTest()
    {
        var environment = new Environment(37.0, 31.0, 19.0, 17.0);
        Assert.Equal(37.0, environment.Left);
        Assert.Equal(56.0, environment.Right);
        Assert.Equal(31.0, environment.Top);
        Assert.Equal(48.0, environment.Bottom);
    }

    [Fact]
    public void WidthTest()
    {
        var environment = new Environment(37.0, 31.0, 19.0, 17.0);
        environment.Width = 23.0;

        Assert.Equal(37.0, environment.Left);
        Assert.Equal(60.0, environment.Right);
        Assert.Equal(31.0, environment.Top);
        Assert.Equal(48.0, environment.Bottom);
    }

    [Fact]
    public void HeightTest()
    {
        var environment = new Environment(37.0, 31.0, 19.0, 17.0);
        environment.Height = 23.0;

        Assert.Equal(37.0, environment.Left);
        Assert.Equal(56.0, environment.Right);
        Assert.Equal(31.0, environment.Top);
        Assert.Equal(54.0, environment.Bottom);
    }

    [Fact]
    public void NonCollisionTest()
    {
        var environment = new Environment(37.0, 31.0, 19.0, 17.0);
        var curPos = new Vector(47.0, 39.0);
        var collision = environment.Collision(curPos, curPos);
        Assert.False(collision);
    }

    [Fact]
    public void LeftCollisionTest()
    {
        var environment = new Environment(37.0, 31.0, 19.0, 17.0);
        var curPos = new Vector(36.0, 39.0);
        var collision = environment.Collision(curPos, curPos);
        Assert.True(collision);
        Assert.Equal(37.0, curPos.X);
    }

    [Fact]
    public void RightCollisionTest()
    {
        var environment = new Environment(37.0, 31.0, 19.0, 17.0);
        var curPos = new Vector(57.0, 39.0);
        var collision = environment.Collision(curPos, curPos);
        Assert.True(collision);
        Assert.Equal(56.0, curPos.X);
    }

    [Fact]
    public void TopCollisionTest()
    {
        var environment = new Environment(37.0, 31.0, 19.0, 17.0);
        var curPos = new Vector(47.0, 49.0);
        var collision = environment.Collision(curPos, curPos);
        Assert.True(collision);
        Assert.Equal(48.0, curPos.Y);
    }

    [Fact]
    public void BottomCollisionTest()
    {
        var environment = new Environment(37.0, 31.0, 19.0, 17.0);
        var curPos = new Vector(47.0, 30.0);
        var collision = environment.Collision(curPos, curPos);
        Assert.True(collision);
        Assert.Equal(31.0, curPos.Y);
    }
}