using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlphaS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.Tests
{
    [TestClass()]
    public class StaticClassTests
    {
        [TestMethod()]
        public void getDateTimeFromStringMKTest()
        {
            Assert.AreEqual("105/5/13".getDateTimeFromStringMK(), "2016/5/13".getDateTimeFromString());
        }
        [TestMethod()]
        public void getDateTimeFromStringMKTest2()
        {
            Assert.AreEqual("76/5/13".getDateTimeFromStringMK(), "1987/5/13".getDateTimeFromString());
        }
    }
}