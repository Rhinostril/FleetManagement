using Microsoft.VisualStudio.TestTools.UnitTesting;
using FleetManagement.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Business.Tests
{
    [TestClass()]
    public class FuelCardTests
    {
        [TestMethod()]
        public void FuelCardTestCtor()
        {
            FuelCard f = new FuelCard("123", new DateTime(1997,03,20), "1324", new List<FuelType> { new FuelType("gasoline") },true);
            Assert.AreEqual("123", f.CardNumber);
            Assert.AreEqual(new DateTime(1997, 03, 20), f.ValidityDate);
            Assert.AreEqual("1324", f.Pin);
           }
        [TestMethod()]
        public void FuelCardTestCtor_setFuelCardiD()
        {
            FuelCard f = new FuelCard(1,"123",new DateTime(1980,08,16),"9876",new List<FuelType> { new FuelType("diesel")},true);
         
            Assert.AreEqual(1, f.FuelCardId);
            Assert.AreEqual("123", f.CardNumber);
            Assert.AreEqual(new DateTime(1980, 08, 16), f.ValidityDate);
            Assert.AreEqual("9876", f.Pin);
            Assert.AreEqual(true, f.IsEnabled);

            

        }
        [TestMethod()]
        public void FuelCardTestCtor_setDriver()
        {
            FuelCard f = new FuelCard(1, "123", new DateTime(1980, 08, 16), "9876", new List<FuelType> { new FuelType("diesel") }, new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" }), true);
            Assert.AreEqual(1, f.FuelCardId);
            Assert.AreEqual("123", f.CardNumber);
            Assert.AreEqual(new DateTime(1980, 08, 16), f.ValidityDate);
            Assert.AreEqual("9876", f.Pin);

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

        [TestMethod()]
        public void Test_SetDriver_Valid()
        {
            FuelCard f = new FuelCard("123", new DateTime(1997, 03, 20), "1324", new List<FuelType> { new FuelType("gasoline") }, true);
            Driver driver = new Driver("Jimmy", "Neutron");
            f.SetDriver(driver);
            Assert.IsNotNull(f.Driver);
        }

        [TestMethod()]
        public void Test_SetDriver_Invalid()
        {

        }


        [TestMethod()]
        public void Test_HasDriver_Valid()
        {

        }

        [TestMethod()]
        public void Test_HasDriver_Invalid()
        {

        }

        [TestMethod()]
        public void Test_RemoveDriver_Valid()
        {
            FuelCard f = new FuelCard("123", new DateTime(1997, 03, 20), "1324", new List<FuelType> { new FuelType("gasoline") }, true);

        }


    }
}