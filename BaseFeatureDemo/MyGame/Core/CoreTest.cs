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
    }
}
