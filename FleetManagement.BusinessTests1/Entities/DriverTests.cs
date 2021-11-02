using Microsoft.VisualStudio.TestTools.UnitTesting;
using FleetManagement.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace FleetManagement.Business.Tests
{
    [TestClass()]
    public class DriverTests
    {
        [Fact]
        public void Test_Constructor_Valid()
        {
            Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78");
            Assert.Equal("Elvis", driver.FirstName);
            Assert.Equal("Presley", driver.LastName);
            Assert.Equal("97.05.20-327.78",driver.SecurityNumber);
            Assert.Equal(new DateTime(1997,05,20), driver.DateOfBirth);
        }
        [Fact]
        public void Test_Constructor2_Valid() {
            Driver driver = new Driver("Donald", "Duck", new DateTime(1997, 05, 24), "97.05.20-327.78", new List<string> { "B", "A1" });
            Assert.Equal("Donald", driver.FirstName);
            Assert.Equal("Duck", driver.LastName);
            Assert.Equal("97.05.20-327.78", driver.SecurityNumber);
            Assert.Equal(new DateTime(1997, 05, 24), driver.DateOfBirth);
            Assert.Equal("B",driver.DriversLicenceType[0]);
            Assert.Equal("A1", driver.DriversLicenceType[1]);
        }
        [Fact]
        public void Test_Constructor3_Valid() {
            Vehicle vehicle = new Vehicle(1, "Porsche", "GT2RS", "1234-1234-1234-17", "KAPPER FURKAN", new List<FuelType> { new FuelType("gasoline") }, "Sportauto", "Donkergrijs", 2);
            Driver driver = new Driver("Dagobert", "Duck", new DateTime(1967, 05, 24), "97.05.20-327.78", new List<string> { "B", "A1" }, vehicle);
            Assert.Equal("Dagobert", driver.FirstName);
            Assert.Equal("Duck", driver.LastName);
            Assert.Equal("97.05.20-327.78", driver.SecurityNumber);
            Assert.Equal(new DateTime(1967, 05, 24), driver.DateOfBirth);
            Assert.Equal("B", driver.DriversLicenceType[0]);
            Assert.Equal("A1", driver.DriversLicenceType[1]);
            Assert.Equal(vehicle.ChassisNumber, driver.Vehicle.ChassisNumber);
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
            Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });

            Address a = new Address(1, "KeizerKarel Straat", "1323", "9000", "Gent", "Belgium");
            driver.SetAddress(a);
            driver.RemoveAddress(a);

            Assert.Null(driver.Address);


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
            Vehicle vehicle = new Vehicle(1, "Porsche", "GT2RS", "1234-1234-1234-17", "KAPPER FURKAN", new List<FuelType> { new FuelType("gasoline") }, "Sportauto", "Donkergrijs", 2, driver);
          
            Assert.NotNull(vehicle);

        }
        [Fact]
        public void SetVehicleTestVersion2() {
            Driver driverorig = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.10.27-363.61", new List<string> { "B", "A1" });
            Driver drivernext= new Driver("Bradley", "Cooper", new DateTime(1977, 05, 10), "97.10.27-363.61", new List<string> { "B", "A1" });
            
            Vehicle vehicleOrig = new Vehicle(1, "Porsche", "GT2RS", "1234-1234-1234-17", "KAPPER FURKAN", new List<FuelType> { new FuelType("gasoline") }, "Sportauto", "Donkergrijs", 2, driverorig);
            drivernext.SetVehicle(vehicleOrig);

            Assert.Null(driverorig.Vehicle);
            Assert.Equal(drivernext.Vehicle.ChassisNumber, vehicleOrig.ChassisNumber);
            //TODO check extra does original vehicle have next driver;

        }
        [Fact]
        public void SetVehicleTestVersion3() {
            Driver driverorig = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.10.27-363.61", new List<string> { "B", "A1" });
            Driver drivernext = new Driver("Bradley", "Cooper", new DateTime(1977, 05, 10), "97.10.27-363.61", new List<string> { "B", "A1" });

            
            Vehicle vehicleOrig = new Vehicle(1, "Porsche", "GT2RS", "1234-1234-1234-17", "KAPPER FURKAN", new List<FuelType> { new FuelType("gasoline") }, "Sportauto", "Donkergrijs", 2, driverorig);
            Vehicle vehiclesecond = new Vehicle(1, "Porsche", "GT2RS", "1234-1334-1234-12", "KAPPER FURKAN", new List<FuelType> { new FuelType("gasoline") }, "Sportauto", "Donkergrijs", 2, drivernext);

            drivernext.SetVehicle(vehicleOrig);
            driverorig.SetVehicle(vehiclesecond);
            Assert.Equal(driverorig.Vehicle.ChassisNumber, vehiclesecond.ChassisNumber);
            Assert.Equal(drivernext.Vehicle.ChassisNumber, vehicleOrig.ChassisNumber);

        }
        [Fact]
        public void SetVehicleTestVersion4() {
            Driver driverorig = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.10.27-363.61", new List<string> { "B", "A1" });
            Driver drivernext = new Driver("Bradley", "Cooper", new DateTime(1977, 05, 10), "97.10.27-363.61", new List<string> { "B", "A1" });
            //Driver driverNr3 = new Driver("Tronald", "Dump", new DateTime(1947, 05, 10), "97.10.27-363.61", new List<string> { "B", "A3" });

            Vehicle vehicleOrig = new Vehicle(1, "Porsche", "GT2RS", "1234-1234-1234-17", "KAPPER FURKAN", new List<FuelType> { new FuelType("gasoline") }, "Sportauto", "Donkergrijs", 2, driverorig);
            Vehicle vehiclesecond = new Vehicle(1, "Porsche", "GT2RS", "1234-1334-1234-12", "KAPPER FURKAN", new List<FuelType> { new FuelType("gasoline") }, "Sportauto", "Donkergrijs", 2, drivernext);
            //Vehicle vehiclenr3 = new Vehicle(1, "Porsche", "GT2RS", "1234-1234-1234-17", "KAPPER FURKAN", new FuelType("Gasoline"), "Sportauto", "Donkergrijs", 2, driverorig);

            drivernext.SetVehicle(vehicleOrig);
            driverorig.SetVehicle(vehiclesecond);
            driverorig.SetVehicle(vehicleOrig);
           Assert.Equal(driverorig.Vehicle.ChassisNumber, vehicleOrig.ChassisNumber);
            Assert.Null(vehiclesecond.Driver);

        }
        [Fact]
        public void SetVehicleTestVersion5() {
            Driver driverorig = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.10.27-363.61", new List<string> { "B", "A1" });
            Driver drivernext = new Driver("Bradley", "Cooper", new DateTime(1977, 05, 10), "97.10.27-363.61", new List<string> { "B", "A1" });
            Driver driverNr3 = new Driver("Tronald", "Dump", new DateTime(1947, 05, 10), "97.10.27-363.61", new List<string> { "B", "A3" });

            Vehicle vehicleOrig = new Vehicle(1, "Porsche", "GT2RS", "1234-1234-1234-17", "KAPPER FURKAN", new List<FuelType> { new FuelType("gasoline") }, "Sportauto", "Donkergrijs", 2, driverorig);
            Vehicle vehiclesecond = new Vehicle(2, "Porsche", "GT2RS", "1234-1334-1234-12", "KAPPER FURKAN", new List<FuelType> { new FuelType("gasoline") }, "Sportauto", "Donkergrijs", 2, drivernext);
            Vehicle vehiclenr3 = new Vehicle(3, "Porsche", "GT2RS", "0004-1234-1234-17", "KAPPER FURKAN", new List<FuelType> { new FuelType("gasoline") }, "Sportauto", "Donkergrijs", 2, driverorig);

            drivernext.SetVehicle(vehicleOrig);
            driverorig.SetVehicle(vehiclesecond);
            driverorig.SetVehicle(vehicleOrig);
            driverNr3.SetVehicle(vehiclesecond);
            drivernext.SetVehicle(vehiclenr3);

            Assert.Equal(drivernext.Vehicle.ChassisNumber, vehiclenr3.ChassisNumber);
            Assert.Equal(vehiclenr3.Driver.LastName, drivernext.LastName);
            Assert.Equal(driverNr3.Vehicle.ChassisNumber, vehiclesecond.ChassisNumber);
            Assert.Equal(vehiclesecond.Driver.LastName, driverNr3.LastName);
            Assert.Equal(driverorig.Vehicle.ChassisNumber, vehicleOrig.ChassisNumber);
            Assert.Equal(vehicleOrig.Driver.LastName, driverorig.LastName);

        }


        [Fact]
        public void SetFuelCardTest()
        {
            FuelCard f = new FuelCard("123", new DateTime(1997, 03, 20), "1324", new List<FuelType> { new FuelType("gasoline") }, true);
            Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });
            driver.SetFuelCard(f);
            Assert.NotNull(driver.FuelCard);
            Assert.Equal(driver.FuelCard.CardNumber, f.CardNumber);

        }
        [Fact]
        public void SetFuelCardTest2() {
            FuelCard f1= new FuelCard("123", new DateTime(1997, 03, 20), "1324", new List<FuelType> { new FuelType("gasoline") }, true);
            FuelCard f2 = new FuelCard("1333", new DateTime(1957, 03, 20), "1325", new List<FuelType> { new FuelType("diesel") }, true);
            
            Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });
            driver.SetFuelCard(f2);
            Assert.NotNull(driver.FuelCard);
            Assert.True(driver.FuelCard.Pin == "1325");

        }

        

    }
}