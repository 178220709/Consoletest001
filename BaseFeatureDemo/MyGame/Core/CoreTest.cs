using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaseFeatureDemo.MyGame.Core
{
    [TestClass]
  public   class CoreTest
    {
        [TestMethod]
        public void GetLenghtTest()
        {
            var p1 = new Point(0, 0);
            var p2 = new Point(1, 1);
            var result = Util.GetLenght(p1, p2);
        }

        [TestMethod]
        public void GetLenghtTest2()
        {
            var p1 = new Point(0, 0);
            var line = new Line(new Point(1, 1), new Point(1, 2));
            var result = Util.GetLenght(p1, line);
            Assert.IsTrue(Math.Abs(result - 1) < 0.001);
        }
    }
}
