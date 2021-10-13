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
    public class VehicleTests
    {
        
        [TestMethod()]
        public void VehicleTest()
        {
            
          

        }

        [TestMethod()]
        public void SetVehicleIdTest()
        {
            var vecId = 1;
            var time = DateTime.DaysInMonth(2000, 10);


            Vehicle v = new Vehicle(vecId,"",""," ","","","","",5, new Driver("","",new DateTime(20,10,01), "00.06.30-283.52",new List<string>()));

            Assert.AreEqual(1, vecId);

        }

        [TestMethod()]
        public void SetBrandTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void SetModelTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void SetChassisNumberTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void SetLicensePlateTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void SetFuelTypeTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void SetVehicleTypeTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void SetVehicleColorTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void SetVehicleDoorsTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void SetDriverTest()
        {
            throw new NotImplementedException();
        }
    }
}