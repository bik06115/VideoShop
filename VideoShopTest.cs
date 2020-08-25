using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VideoRentShop;

namespace VideoShopUnitTests
{
    [TestClass]
    public class VideoShopTests
    {
        [TestMethod]
        public void Get_RentedMovies_Data_ReturnsDataObject()
        {
            VideoShop ms = new VideoShop();
            DataTable d = ms.ListRentedMovies();
            Assert.IsNotNull(d);
        }
    }
}
