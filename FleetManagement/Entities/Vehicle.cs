using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Exceptions;

namespace FleetManagement.Business.Entities
{
    public class Vehicle
    {
        public int VehicleId { get; private set; }
        public string Brand { get; private set; }
        public string Model { get; private set; }
        public string ChassisNumber { get; private set; }
        public string LicensePlate { get; private set; }
        public string FuelType { get; private set; }
        public string VehicleType { get; private set; }
        public string Color { get; private set; }
        public int Doors { get; private set; }
        public Driver Driver { get; private set; }

        public Vehicle(int vehicleId, string brand, string model, string chassisNumber, string licensePlate, string fuelType, string vehicleType, string color, int doors, Driver driver)
        {
            SetVehicleId(vehicleId);
            SetBrand(brand);
            SetModel(model);
            SetChassisNumber(chassisNumber);
            SetLicensePlate(licensePlate);
            SetFuelType(fuelType);
            SetVehicleType(vehicleType);
            SetVehicleColor(color);
            SetVehicleDoors(doors);
            SetDriver(driver);
        }

        public void SetVehicleId(int id)
        {
            if (id > 0)
            {
                VehicleId = id;
            }
            else
            {
                throw new VehicleException("Vehicle id must be greater than 0!");
            }
        }

        public void SetBrand(string brand)
        {
            if (!string.IsNullOrWhiteSpace(brand))
            {
                Brand = brand;
            }
            else
            {
                throw new VehicleException("Vehicle brand can't be empty!");
            }
        }

        public void SetModel(string model)
        {
            if (!string.IsNullOrWhiteSpace(model))
            {
                Model = model;
            }
            else
            {
                throw new VehicleException("Vehicle brand can't be empty!");
            }
        }

        public void SetChassisNumber(string number)
        {
            if (!string.IsNullOrWhiteSpace(number))
            {
                if(number.Length == 3 + 6 + 8)
                {
                    ChassisNumber = number;
                }
                else
                {
                    throw new VehicleException("Vehicle chassis number must be 17 characters long!");
                }

            }
            else
            {
                throw new VehicleException("Vehicle chassis number can't be empty!");
            }
        }

        public void SetLicensePlate(string plate)
        {
            if (!string.IsNullOrWhiteSpace(plate))
            {
                LicensePlate = plate;
            }
            else
            {
                throw new VehicleException("Vehicle license plate can't be empty!");
            }
        }

        public void SetFuelType(string fuel)
        {
            if (!string.IsNullOrWhiteSpace(fuel))
            {
                FuelType = fuel;
            }
            else
            {
                throw new VehicleException("Vehicle fuel type can't be empty!");
            }
        }
        public void SetVehicleType(string vehicleType)
        {
            if (!string.IsNullOrWhiteSpace(vehicleType))
            {
                VehicleType = vehicleType;
            }
            else
            {
                throw new VehicleException("Vehicle type can't be empty!");
            }
        }
        public void SetVehicleColor(string color)
        {
            if (!string.IsNullOrWhiteSpace(color))
            {
                Color = color;
            }
            else
            {
                throw new VehicleException("Vehicle color can't be empty!");
            }
        }

        public void SetVehicleDoors(int doors)
        {
            if(doors > 0 && doors <= 7)
            {
                Doors = doors;
            }
            else
            {
                throw new VehicleException("Vehicle has to have more than 0 and less or equal than 7 doors!");
            }
        }

        public void SetDriver(Driver driver)
        {
            if(Driver != null) // heeft vehicle al een driver?
            {
                if(Driver != driver)
                {
                    Driver = driver;
                }
            }
            else
            {
                if(driver != null) // is driver niet null
                {
                    Driver = driver;
                }
                else
                {
                    
                }
            }
        }



    }
}
