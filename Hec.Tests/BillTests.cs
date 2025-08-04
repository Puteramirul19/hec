using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hec.Entities;

namespace Hec.Tests
{
    [TestClass]
    public class BillTests
    {
        [TestMethod]
        public void TestBuildMonthYear()
        {
            Assert.AreEqual(201701, Bill.BuildMonthYear(new DateTime(2017, 1, 1)));
            Assert.AreEqual(201706, Bill.BuildMonthYear(new DateTime(2017, 6, 20)));
            Assert.AreEqual(201812, Bill.BuildMonthYear(new DateTime(2018, 12, 30)));
        }

        [TestMethod]
        public void TestCompareMonthYear()
        {
            Assert.IsTrue(Bill.BuildMonthYear(new DateTime(2017, 1, 1)) < Bill.BuildMonthYear(new DateTime(2017, 6, 20)));
            Assert.IsTrue(Bill.BuildMonthYear(new DateTime(2017, 6, 13)) == Bill.BuildMonthYear(new DateTime(2017, 6, 20)));
            Assert.IsTrue(Bill.BuildMonthYear(new DateTime(2017, 12, 13)) > Bill.BuildMonthYear(new DateTime(2017, 6, 20)));
        }
    }
}
