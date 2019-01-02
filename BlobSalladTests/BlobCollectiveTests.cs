using ApprovalTests;
using BlobSallad;
using NUnit.Framework;

namespace BlobSalladTests
{
    public class BlobCollectiveTests
    {
        [Test]
        public void ctorTest()
        {
            BlobCollective collective = new BlobCollective(71.0, 67.0, 4);

            Assert.AreEqual(4, collective.getMaxNum());
            Assert.AreEqual(1, collective.getNumActive());
        }

        //[Test]
        //public void splitTest()
        //{
        //    BlobCollective collective = new BlobCollective(1.0, 1.0, 4);

        //    BufferedImage image = new BufferedImage(200, 200, TYPE_INT_RGB);
        //    Graphics2D graphics = image.createGraphics();
        //    try
        //    {
        //        graphics.setPaint(Color.WHITE);
        //        graphics.fillRect(0, 0, image.getWidth(), image.getHeight());

        //        collective.split();
        //        Assert.AreEqual(2, collective.getNumActive());
        //        collective.draw(graphics, 100.0);
        //        Approvals.verify(image);
        //    }
        //    finally
        //    {
        //        graphics.dispose();
        //    }
        //}

        // [Test]
        //public void joinTest()
        //{
        //    BlobCollective collective = new BlobCollective(1.0, 1.0, 4);

        //    BufferedImage image = new BufferedImage(200, 200, TYPE_INT_RGB);
        //    Graphics2D graphics = image.createGraphics();
        //    try
        //    {
        //        graphics.setPaint(Color.WHITE);
        //        graphics.fillRect(0, 0, image.getWidth(), image.getHeight());

        //        collective.split();
        //        // collective.join();
        //        //Assert.AreEqual(1, collective.getNumActive());
        //        collective.draw(graphics, 100.0);
        //        Approvals.verify(image);
        //    }
        //    finally
        //    {
        //        graphics.dispose();
        //    }
        //}

        [Test]
        public void selectBlobMissTest()
        {
            BlobCollective collective = new BlobCollective(1.0, 1.0, 4);
            collective.selectBlob(2.0, 2.0);
            Assert.IsNull(collective.getSelectedBlob());
        }

        [Test]
        public void selectBlobHitTest()
        {
            BlobCollective collective = new BlobCollective(1.0, 1.0, 4);
            collective.selectBlob(1.0, 1.1);
            Assert.NotNull(collective.getSelectedBlob());
            Assert.True(collective.getSelectedBlob().getSelected());
        }

        //[Test]
        //public void drawTest()
        //{
        //    BlobCollective collective = new BlobCollective(1.0, 1.0, 4);

        //    BufferedImage image = new BufferedImage(200, 200, TYPE_INT_RGB);
        //    Graphics2D graphics = image.createGraphics();
        //    try
        //    {
        //        graphics.setPaint(Color.WHITE);
        //        graphics.fillRect(0, 0, image.getWidth(), image.getHeight());

        //        collective.draw(graphics, 100.0);
        //        Approvals.verify(image);
        //    }
        //    finally
        //    {
        //        graphics.dispose();
        //    }
        //}
    }
}