using Microsoft.VisualStudio.TestTools.UnitTesting;
using FleetManagement.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace FleetManagement.Business.Entities.Tests
{
    [TestClass()]
    public class DriverTests
    {
        [Fact]
        public void Test_Constructor_Valid()
        {
            Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });
            Assert.Equal("Elvis", driver.FirstName);
            Assert.Equal("Presley", driver.LastName);
            Assert.Equal("97.05.20-327.78",driver.SecurityNumber);
            Assert.Equal(new DateTime(1997,05,20), driver.DateOfBirth);
            Assert.Equal(new List<string>{"B","A1"}, driver.DriversLicenceType);
        }

        [Fact]
        public void SetSecurityNumberTest()
        {
            Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });
            Assert.Equal("97.05.20-327.78", driver.SecurityNumber);
        }

        [Fact]
        public void AddTypeToDriversLicenseTest()
        {
            
        }

        [Fact]
        public void SetAddressTest()
        {
            Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });

            Address a = new Address();
            a.SetStreet("street"); //a.Street = "street";
            a.SetPostalCode("9000"); //a.PostalCode = "9000";
            a.SetHouseNr("2"); //a.HouseNr = "2";
            a.SetCountry("belgium"); //a.Country = "belgium";
            a.SetCity("gent"); //a.City = "gent";

            driver.SetAddress(a);

            Assert.NotNull(a.Street);
            Assert.NotNull(a.PostalCode);
            Assert.NotNull(a.HouseNr);
            Assert.NotNull(a.Country);
            Assert.NotNull(a.City);
           
        }

        [Fact]
        public void RemoveAddressTest()
        {
            //Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });

            //Address a = new Address();
            //a.Street = "street";
            //a.PostalCode = "9000";
            //a.HouseNr = "2";
            //a.Country = "belgium";
            //a.City = "gent";
            //a.AddressID = 1;
            //driver.setAddress(a);
            //driver.removeAddress(a);

            //Assert.Null(a);


        }

        [Fact]
        public void SetDriverIDTest()
        {
            Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });
            driver.SetDriverID(1);
            Assert.Equal(1, driver.DriverID);

        }

        [Fact]
        public void SetVehicleTest()
        {
            Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });
            Vehicle vehicle = new Vehicle(1, "Porsche", "GT2RS", "1234-1234-1234-17", "KAPPER FURKAN", "Gasoline", "Sportauto", "Donkergrijs", 2, null);
            driver.SetVehicle(vehicle);
            Assert.NotNull(vehicle);

        }

        [Fact]
        public void TryRemoveVehicleTest()
        {
            Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });
            Vehicle vehicle = new Vehicle(1, "Porsche", "GT2RS", "1234-1234-1234-17", "KAPPER FURKAN", "Gasoline", "Sportauto", "Donkergrijs", 2, null);
            driver.SetVehicle(vehicle);
            Assert.True(driver.TryRemoveVehicle(vehicle));

        }

        [Fact]
        public void SetFuelCardTest()
        {
            FuelCard f = new FuelCard(1, "123", new DateTime(), "1324", null);
            Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });
            driver.SetFuelCard(f);
            Assert.NotNull(f);

        }

        [Fact]
        public void RemoveAnyFuelCardTest()
        {
            Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });
            Vehicle vehicle = new Vehicle(1, "Porsche", "GT2RS", "1234-1234-1234-17", "KAPPER FURKAN", "Gasoline", "Sportauto", "Donkergrijs", 2, null);
            FuelCard f = new FuelCard(1, "123", new DateTime(), "1324", null);

            driver.SetFuelCard(f);


            Assert.NotNull(f);

        }

        [Fact]
        public void TryRemoveSpecificFuelCardTest()
        {
            Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });
            Vehicle vehicle = new Vehicle(1, "Porsche", "GT2RS", "1234-1234-1234-17", "KAPPER FURKAN", "Gasoline", "Sportauto", "Donkergrijs", 2, null);
            FuelCard f1 = new FuelCard(1, "123", new DateTime(), "1324", null);
            FuelCard f2 = new FuelCard(1, "123", new DateTime(), "1324", null);

            driver.SetFuelCard(f2);
            driver.SetFuelCard(f1);

            driver.TryRemoveSpecificFuelCard(f1);

            Assert.NotNull(f1);

        }
    }
}