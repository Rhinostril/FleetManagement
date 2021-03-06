using Xunit;
using FleetManagement.Business.Entities;
using FleetManagement.Business.Exceptions;

namespace FleetManagement.Business.Tests
{

    public class FuelTypeTests
    {
        //TODO other constructors
        [Fact]
        public void Test_SetFuelName_Valid()
        {
            FuelType fuelType = new FuelType("Gasoline");
            fuelType.SetFuelName("Diesel");
            Assert.Equal("Diesel", fuelType.FuelName);
        }
        [Fact]
        public void Test_SetFuelId_valid()
        {
            FuelType fuelType = new FuelType(1,"Gasoline");
            fuelType.SetFuelName("Diesel");
            Assert.Equal(1, fuelType.FuelTypeId);
            Assert.Equal("Diesel", fuelType.FuelName);

        }


        [Fact]
        public void Test_SetFuelName_Invalid()
        {
            FuelType fuelType = new FuelType("Gasoline");
            Assert.Throws<FuelTypeException>(() => fuelType.SetFuelName(""));
        }


    }
}