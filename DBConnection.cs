using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VideoRentShop;

namespace VideoShopUnitTests
{
    [TestClass]
    public class Test_Connection_DB
    {
        [TestMethod]
        public void Test_Connection_DB_OpenSate_ReturnsTrue()
        {
            VideoShop ms = new VideoShop();
            bool v = ms.TestConnectionDatabase();
            Assert.IsTrue(v);
        }
    }
}
