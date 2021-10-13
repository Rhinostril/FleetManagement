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
    public class FuelTypeTests
    {
        [TestMethod()]
        public void GetFuelTypeTest()
        {
            FuelType f = new FuelType();
            Assert.AreEqual("Diesel", f.GetFuelType(1));
        }

 
    }
}