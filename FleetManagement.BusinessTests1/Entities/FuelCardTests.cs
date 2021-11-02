using Microsoft.VisualStudio.TestTools.UnitTesting;
using FleetManagement.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Business.Entities.Tests
{
    [TestClass()]
    public class FuelCardTests
    {
        [TestMethod()]
        public void FuelCardTest()
        {
            FuelCard f = new FuelCard("123", new DateTime(1997,03,20), "1324", new List<FuelType> { new FuelType("gasoline") },true);
            Assert.AreEqual(1, f.FuelCardId);
            Assert.AreEqual("123", f.CardNumber);
            Assert.AreEqual(new DateTime(1997, 03, 20), f.ValidityDate);
            Assert.AreEqual("1324", f.Pin);
           }

        [TestMethod()]
        public void SetFuelCardIdTest()
        {
            FuelCard f = new FuelCard(1,"123", new DateTime(1997, 03, 20), "1324", new List<FuelType> { new FuelType("gasoline") }, true);
            Assert.AreEqual(1, f.FuelCardId);
        }

        [TestMethod()]
        public void SetCardNumberTest()
        {
            FuelCard f = new FuelCard("123", new DateTime(1997, 03, 20), "1324", new List<FuelType> { new FuelType("gasoline") }, true);
            Assert.AreEqual("123", f.CardNumber);
        }

        [TestMethod()]
        public void SetPinTest()
        {
            FuelCard f = new FuelCard("123", new DateTime(1997, 03, 20), "1324", new List<FuelType> { new FuelType("gasoline") }, true);
            Assert.AreEqual("1324", f.Pin);
        }

    }
}